using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace Rpsls.Tasks.Infrastructure
{
	public static class TaskExecutor
	{
		private static readonly ThreadLocal<List<BackgroundTask>> tasksToExecute =
			new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

		public static IDocumentStore DocumentStore
		{
			get 
			{
				if (_documentStore == null)
				{
					var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
					parser.Parse();

					_documentStore = new DocumentStore
					{
						ApiKey = parser.ConnectionStringOptions.ApiKey,
						Url = parser.ConnectionStringOptions.Url
					};

					_documentStore.Initialize();
				}

				return _documentStore; 
			}
			//set
			//{
			//	if (_documentStore == null)
			//	{
			//		_documentStore = value;
			//	}
			//}
		}
		private static IDocumentStore _documentStore;

		public static Action<Exception> ExceptionHandler { get; set; }

		public static void ExecuteLater(BackgroundTask task)
		{
			tasksToExecute.Value.Add(task);
		}

		public static void Discard()
		{
			tasksToExecute.Value.Clear();
		}

		public static void StartExecuting()
		{
			var value = tasksToExecute.Value;
			var copy = value.ToArray();
			value.Clear();

			if (copy.Length > 0)
			{
				Task.Factory.StartNew(() =>
				{
					foreach (var backgroundTask in copy)
					{
						ExecuteTask(backgroundTask);
					}
				}, TaskCreationOptions.LongRunning)
					.ContinueWith(task =>
					{
						if (ExceptionHandler != null) ExceptionHandler(task.Exception);
					}, TaskContinuationOptions.OnlyOnFaulted);
			}
		}

		public static void ExecuteTask(BackgroundTask task)
		{
			for (var i = 0; i < 10; i++)
			{
				using (var session = DocumentStore.OpenSession())
				{
					switch (task.Run(session, _documentStore))
					{
						case true:
						case false:
							return;
						case null:
							break;
					}
				}
			}
		}
	}
}