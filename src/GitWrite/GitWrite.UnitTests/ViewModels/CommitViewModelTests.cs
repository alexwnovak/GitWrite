﻿using GitModel;
using GitWrite.Services;
using GitWrite.ViewModels;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Fact]
      public void AcceptCommand_Executing_SavesCommitDetails()
      {
         var commitDocument = new CommitDocument();
         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", commitDocument, commitFileWriterMock.Object, Mock.Of<IViewService>() );
         viewModel.AcceptCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", commitDocument ), Times.Once() );
      }

      [Fact]
      public void AcceptCommand_Executing_DismissesTheView()
      {
         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, null, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseView(), Times.Once() );
      }
   }
}
