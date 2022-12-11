namespace Exam.Core.Utility
{
    public class Utitity
    {  
        /// <summary>
        /// The method gets file path and return it as a string
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFilePath(string folderName, string fileName)
        {
            return Path.Combine(folderName, fileName);
        }
    }
}
