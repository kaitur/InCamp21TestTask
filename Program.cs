using InCamp21TestTask.processors;

namespace InCamp21TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
             new TransformTable("InputFile.csv").TableFormation();
        }
    }
}
