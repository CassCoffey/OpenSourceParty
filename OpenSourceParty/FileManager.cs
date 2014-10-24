/*  Open Source Party, a platform for making and playing party minigames with your friends
    Copyright (C) 2014  Sean Coffey

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
using System;
using System.Collections.Generic;
using System.IO;

namespace FileHandler
{
    public static class FileManager
    {
        // Fields
        private static List<String> filePaths = new List<String>();   // List of files that were found using directorySearch.
        private const String GAMEEXTENSION = "*.mng";   // The default minigame extension.
        private const String IMAGEEXTENSION = "*.jpg";   // The default minigame extension.
        private const String MINIGAMEDIR = "./MiniGames";   // The default minigame directory.
        private const String BACKGROUNDDIR = "./Backgrounds";   // The default minigame directory.
        private const String IMAGEDIR = "./Images";   // The default minigame directory.
        private static Random rand = new Random();


        // Properties
        public static String GameExtension
        {
            get
            {
                return GAMEEXTENSION;
            }
        }
        public static String ImageExtension
        {
            get
            {
                return IMAGEEXTENSION;
            }
        }
        public static String MinigameDir
        {
            get
            {
                return MINIGAMEDIR;
            }
        }
        public static String BackgroundDir
        {
            get
            {
                return BACKGROUNDDIR;
            }
        }
        public static String ImageDir
        {
            get
            {
                return IMAGEDIR;
            }
        }


        // Methods and Constructors
        /// <summary>
        /// Finds and runs a random game in the specified directory.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        public static String RandomFile(String dir = MINIGAMEDIR, String ext = GAMEEXTENSION)
        {
            filePaths.Clear();
            directorySearch(dir, ext);
            if (filePaths.Count == 0)
            {
                return null;
            }
            return filePaths[rand.Next(0, filePaths.Count)];   // Pick a random file from the files list.
        }

        /// <summary>
        /// Returns the path of a file whose name is specified.
        /// </summary>
        /// <param name="name">The name of the file to search for.</param>
        /// <param name="dir">The directory to search in.</param>
        /// <param name="ext">The extension of the file.</param>
        /// <returns>The path of the file, if found.</returns>
        public static String NamedFile(String name, String dir = MINIGAMEDIR, String ext = GAMEEXTENSION)
        {
            filePaths.Clear();
            directorySearch(dir, ext);
            if (filePaths.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < filePaths.Count; i++)
            {
                if (filePaths[i].Contains(name))
                {
                    return filePaths[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively search directories for the specified file type.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        /// <param name="ext">The extension to search for.</param>
        private static void directorySearch(String dir, String ext = "*")
        {
            try
            {
                foreach (String file in Directory.GetFiles(dir, ext))   // Check the current directory for files, and add them to the list.
                {
                    filePaths.Add(file);
                }
                foreach (String directory in Directory.GetDirectories(dir))   // Recursively do the same for all directories within the current directory.
                {
                    directorySearch(directory, ext);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /// <summary>
        /// An overload of the directorySearch method that also takes a file name to search for.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <param name="dir">The directory to begin searching in.</param>
        /// <param name="ext">The extension of the file.</param>
        private static void directorySearch(String name, String dir, String ext = "*")
        {
            try
            {
                foreach (String file in Directory.GetFiles(dir, ext))   // Check the current directory for files, and add them to the list.
                {
                    if (file.Contains(name))
                    {
                        filePaths.Add(file);
                    }
                }
                foreach (String directory in Directory.GetDirectories(dir))   // Recursively do the same for all directories within the current directory.
                {
                    directorySearch(directory, ext);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /// <summary>
        /// Prints all of the files found in the specified directory.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        /// <param name="ext">The extension to search for.</param>
        public static void printFileList(String dir, String ext = "*")
        {
            filePaths.Clear();
            directorySearch(dir, ext);
            if (filePaths.Count == 0)
            {
                return;
            }
            for (int i = 0; i < filePaths.Count; i++)
            {
                Console.WriteLine(filePaths[i]);
            }
        }
    }
}
