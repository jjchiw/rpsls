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

			var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
			parser.Parse();

			var documentStore = new DocumentStore
			{
				ApiKey = parser.ConnectionStringOptions.ApiKey,
				Url = parser.ConnectionStringOptions.Url
			};

			documentStore.Initialize();

			IndexCreation.CreateIndexes(typeof(MatchEncounterLoseIndex).Assembly, documentStore);
			IndexCreation.CreateIndexes(typeof(MatchEncounterWinIndex).Assembly, documentStore);
			IndexCreation.CreateIndexes(typeof(MatchEncounterTieIndex).Assembly, documentStore);

			container.Register<IDocumentStore>(documentStore);
			container.Register<IUserMapper, UserMapper>();

			context.Items["RavenDocumentStore"] = documentStore;
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