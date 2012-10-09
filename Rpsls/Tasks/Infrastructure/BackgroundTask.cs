using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace Rpsls.Tasks.Infrastructure
{
	public abstract class BackgroundTask
	{
		protected IDocumentSession DocumentSession;
		protected IDocumentStore DocumentStore;

		private readonly Logger logger = LogManager.GetCurrentClassLogger();

		protected virtual void Initialize(IDocumentSession session, IDocumentStore documentStore)
		{
			DocumentSession = session;
			DocumentStore = documentStore;
			DocumentSession.Advanced.UseOptimisticConcurrency = false;
		}

		protected virtual void OnError(Exception e)
		{
		}

		public bool? Run(IDocumentSession openSession, IDocumentStore documentStore)
		{
			Initialize(openSession, documentStore);
			try
			{
				Execute();
				DocumentSession.SaveChanges();
				TaskExecutor.StartExecuting();
				return true;
			}
			catch (ConcurrencyException e)
			{
				logger.ErrorException("Could not execute task " + GetType().Name, e);
				OnError(e);
				return null;
			}
			catch (Exception e)
			{
				logger.ErrorException("Could not execute task " + GetType().Name, e);
				OnError(e);
				return false;
			}
			finally
			{
				TaskExecutor.Discard();
			}
		}

		public abstract void Execute();
	}
}