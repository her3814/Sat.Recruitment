using System.IO;

namespace Sat.Recruitment.Test.Helpers
{
    public static class TestFileHelper
    {
        public static void ResetTestFile()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "/Files/Users.txt"))
            {
                File.Delete(Directory.GetCurrentDirectory() + "/Files/Users.txt");
                File.Copy(Directory.GetCurrentDirectory() + "/Files/Users.test.txt", Directory.GetCurrentDirectory() + "/Files/Users.txt");
            }
            else File.Copy(Directory.GetCurrentDirectory() + "/Files/Users.test.txt", Directory.GetCurrentDirectory() + "/Files/Users.txt");

        }
    }
}
