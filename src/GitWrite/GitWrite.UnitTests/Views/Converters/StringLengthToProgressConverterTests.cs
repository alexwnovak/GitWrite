﻿using Xunit;
using FluentAssertions;
using GitWrite.Views.Converters;

namespace GitWrite.UnitTests.Views.Converters
{
   public class StringLengthToProgressConverterTests
   {
      [Fact]
      public void Convert_PassesNullValue_ReturnsZero()
      {
         var converter = new StringLengthToProgressConverter();

         double progress = (double) converter.Convert( null, null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesInEmptyString_ReturnsZero()
      {
         var converter = new StringLengthToProgressConverter();

         double progress = (double) converter.Convert( string.Empty, null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesUnparsableString_ReturnsZero()
      {
         var converter = new StringLengthToProgressConverter();

         double progress = (double) converter.Convert( "NotAnInteger", null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesStringWithZeroInteger_ReturnsProgressValue()
      {
         var converter = new StringLengthToProgressConverter();

         double progress = (double) converter.Convert( "0", null, null, null );

         progress.Should().Be( 0.0 );
      }

      [Fact]
      public void Convert_PassesStringWith72Integer_ReturnsProgressValue()
      {
         var converter = new StringLengthToProgressConverter();

         double progress = (double) converter.Convert( "72", null, null, null );

         progress.Should().Be( 1.0 );
      }
   }
}