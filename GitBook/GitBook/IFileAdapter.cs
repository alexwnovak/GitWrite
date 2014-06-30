﻿namespace GitBook
{
   public interface IFileAdapter
   {
      bool Exists( string path );

      string[] ReadAllLines( string path );
   }
}
