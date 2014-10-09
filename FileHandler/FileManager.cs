﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace FileHandler
{
    public class FileManager
    {
        // Fields
        private List<String> filePaths = new List<String>();   // List of files that were found using directorySearch.
        private const String GAMEEXTENSION = "*.mng";   // The default minigame extension.
        private const String IMAGEEXTENSION = "*.jpg";   // The default minigame extension.
        private const String MINIGAMEDIR = ".\\MiniGames";   // The default minigame directory.
        private const String BACKGROUNDDIR = ".\\Backgrounds";   // The default minigame directory.
        private const String IMAGEDIR = ".\\Images";   // The default minigame directory.
        private static Random rand = new Random();


        // Properties
        public String GameExtension
        {
            get
            {
                return GAMEEXTENSION;
            }
        }
        public String ImageExtension
        {
            get
            {
                return IMAGEEXTENSION;
            }
        }
        public String MinigameDir
        {
            get
            {
                return MINIGAMEDIR;
            }
        }
        public String BackgroundDir
        {
            get
            {
                return BACKGROUNDDIR;
            }
        }
        public String ImageDir
        {
            get
            {
                return IMAGEDIR;
            }
        }
        public String this[int index]
        {
            get
            {
                if (index >= 0 && index <= filePaths.Count)
                {
                    return filePaths[index];
                }
                else
                {
                    return null;
                }
            }
        }


        // Methods and Constructors
        /// <summary>
        /// Finds and runs a random game in the specified directory.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        public String RandomFile(String dir = MINIGAMEDIR, String ext = GAMEEXTENSION)
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
        public String NamedFile(String name, String dir = MINIGAMEDIR, String ext = GAMEEXTENSION)
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
        /// Finds a random game file and runs it.
        /// </summary>
        /// <param name="dir">The directory to search for the game in.</param>
        /// <param name="ext">The extension to search for.</param>
        public void RandomGame(String dir = MINIGAMEDIR, String ext = GAMEEXTENSION)
        {
            String randomGame = RandomFile(dir, ext);

            if (filePaths.Count == 0)
            {
                return;
            }

            var DLL = Assembly.LoadFile(Path.GetFullPath(randomGame));   // Load that file.

            foreach (Type type in DLL.GetExportedTypes())   // For every 'type' found in the .dll...
            {
                dynamic c = Activator.CreateInstance(type);   // Create an instance of that type...
                c.TestMessage();   // Attempt to run a TestMessage()
            }
        }

        /// <summary>
        /// Recursively search directories for the specified file type.
        /// </summary>
        /// <param name="dir">The directory to search.</param>
        /// <param name="ext">The extension to search for.</param>
        private void directorySearch(String dir, String ext = "*")
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
        private void directorySearch(String name, String dir, String ext = "*")
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
        public void printFileList(String dir, String ext = "*")
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

        /// <summary>
        /// Made specifically for the Sprite classes. Gets a list of images with the same string in their name.
        /// </summary>
        /// <param name="name">The string in common with all the images.</param>
        /// <param name="dir">The directory to search in.</param>
        /// <param name="ext">The extension, this will almost always be Image Extension.</param>
        /// <returns>A list of images with the common string in their name.</returns>
        public List<Image> GetNamedImageList(String name, String dir = IMAGEDIR, String ext = IMAGEEXTENSION)
        {
            List<Image> tempList = new List<Image>();
            filePaths.Clear();
            directorySearch(name, dir, ext);
            if (filePaths.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < filePaths.Count; i++)
            {
                Image newImage = Image.FromFile(filePaths[i]);
                tempList.Add(newImage);
            }
            return tempList;
        }
    }
}