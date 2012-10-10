using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Forms;
using Nancy;
using TinyIoC;
using Raven.Client;
using Raven.Abstractions.Data;
using Raven.Client.Document;
using Nancy.Bootstrapper;
using System.Text;
using Nancy.Cryptography;
using Nancy.Session;
using Raven.Client.Indexes;
using Rpsls.Helpers.Indexes;
using Rpsls.Models;
using Rpsls.Models.ViewModels;
using AutoMapper;
using Rpsls.Helpers;
using Rpsls.Tasks;

namespace Rpsls.Helpers
{
	public class RpslsBootStrapper : DefaultNancyBootstrapper
	{

		private static readonly byte[] bytes = Encoding.UTF8.GetBytes("estoesunapinchementadademadre");
		private static readonly string phrase = "mellevalamierda";
		private static readonly IKeyGenerator keygen = new PassphraseKeyGenerator(phrase, bytes);
		private static readonly Lazy<CryptographyConfiguration> RpslDefaultConfiguration =
			new Lazy<CryptographyConfiguration>(() => new CryptographyConfiguration(
														  new RijndaelEncryptionProvider(keygen),
														  new DefaultHmacProvider(keygen)));

		protected override Nancy.Cryptography.CryptographyConfiguration CryptographyConfiguration
		{
			get
			{
				return RpslDefaultConfiguration.Value;
			}
		}

		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			CookieBasedSessions.Enable(pipelines);


			Mapper.CreateMap<MatchEncounter, MatchEncounterView>()
				  .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.FriendlyParse()))
				  .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result.ToString()))
				  .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.UserName))
				  .ForMember(dest => dest.UserRival, opt => opt.MapFrom(src => src.UserRival.UserName))
				  .ForMember(dest => dest.UserGesture, opt => opt.MapFrom(src => src.UserGestureType.ToString()))
				  .ForMember(dest => dest.UserRivalGesture, opt => opt.MapFrom(src => src.UserRivalGestureType.ToString()))
				  .ForMember(dest => dest.UserRivalId, opt => opt.MapFrom(src => src.UserRival.Id));

			Mapper.CreateMap<User, UserView>()
				  .ForMember(dest => dest.Token, opt => opt.Ignore());

			var documentStore = ConfigureInitializeDocumentStore();

			IndexCreation.CreateIndexes(typeof(MatchEncounterIndex).Assembly, documentStore);
			IndexCreation.CreateIndexes(typeof(MatchEncountersByUserId).Assembly, documentStore);

			SeedBadges.Execute(documentStore);

			documentStore.Dispose();
		}

		protected override void ConfigureApplicationContainer(TinyIoC.TinyIoCContainer container)
		{
			// We don't call "base" here to prevent auto-discovery of
			// types/dependencies
		}

		protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
		{
			base.ConfigureRequestContainer(container, context);


			// Here we register our user mapper as a per-request singleton.
			// As this is now per-request we could inject a request scoped
			// database "context" or other request scoped services.

			var documentStore = ConfigureInitializeDocumentStore();

			container.Register<IDocumentStore>(documentStore);
			container.Register<IUserMapper, UserMapper>();

			context.Items["RavenDocumentStore"] = documentStore;
		}

		private IDocumentStore ConfigureInitializeDocumentStore()
		{
			var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
			parser.Parse();

			var documentStore = new DocumentStore
			{
				ApiKey = parser.ConnectionStringOptions.ApiKey,
				Url = parser.ConnectionStringOptions.Url
			};

			documentStore.Initialize();

			return documentStore;
		}


		protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
		{
			// At request startup we modify the request pipelines to
			// include forms authentication - passing in our now request
			// scoped user name mapper.
			//
			// The pipelines passed in here are specific to this request,
			// so we can add/remove/update items in them as we please.
			var formsAuthConfiguration =
				new FormsAuthenticationConfiguration()
				{
					RedirectUrl = "/",
					UserMapper = requestContainer.Resolve<IUserMapper>(),
				};
			FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
		}
	}
}