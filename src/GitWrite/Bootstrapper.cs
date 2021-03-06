﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using GitModel;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class Bootstrapper : BootstrapperBase
   {
      private SimpleContainer _container = new SimpleContainer();

      public Bootstrapper()
      {
         Initialize();
      }

      protected override object GetInstance( Type service, string key ) => _container.GetInstance( service, key );
      protected override IEnumerable<object> GetAllInstances( Type service ) => _container.GetAllInstances( service );
      protected override void BuildUp( object instance ) => _container.BuildUp( instance );

      protected override void Configure()
      {
         _container.Singleton<IWindowManager, WindowManager>();

         _container.PerRequest<CommitViewModel>();
         _container.Handler<GetCommitFilePathFunction>( c => new GetCommitFilePathFunction( () => Environment.GetCommandLineArgs().Last() ) );
         _container.Handler<ReadCommitFileFunction>( c => new ReadCommitFileFunction( filePath => new CommitFileReader().FromFile( filePath ) ) );
         _container.Handler<WriteCommitFileFunction>( c => new WriteCommitFileFunction( ( filePath, document ) => new CommitFileWriter().ToFile( filePath, document ) ) );
         _container.Handler<ConfirmExitFunction>( c => new ConfirmExitFunction( ViewService.ConfirmExit ) );
      }

      protected override void OnStartup( object sender, StartupEventArgs e )
      {
         dynamic settings = new ExpandoObject();
         settings.ResizeMode = ResizeMode.NoResize;
         settings.WindowStartupLocation = WindowStartupLocation.Manual;
         settings.Width = 812;
         settings.Height = 70;
         settings.Top = 0.3 * SystemParameters.PrimaryScreenHeight;
         settings.Left = ( SystemParameters.PrimaryScreenWidth - settings.Width ) / 2;

         DisplayRootViewFor<CommitViewModel>( settings );
      }
   }
}
