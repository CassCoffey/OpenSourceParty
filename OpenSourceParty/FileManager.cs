using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSourceParty
{
    class FileManager
    {
        // Fields
        private List<String> filePaths = new List<String>();   // List of files that were found using directorySearch.
        private const String GAMEEXTENSION = "*.mng";   // The default minigame extension.
        private const String IMAGEEXTENSION = "*.jpg";   // The default minigame extension.
        private const String MINIGAMEDIR = ".\\MiniGames";   // The default minigame directory.
        private const String BACKGROUNDDIR = ".\\Backgrounds";   // The default minigame directory.
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
    }
}
