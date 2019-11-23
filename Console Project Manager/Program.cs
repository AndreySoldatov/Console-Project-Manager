/*
 * Created by Andrey Soldatov
 * 23.11.2019
 * version 1.0
 * Console Task manager
 */

using System;
using System.IO;

namespace Console_Project_Manager
{
    struct Project
    {
        public string ProjectName;
        public string Done;
        public string UnDone;
        public string InProgress;
        public string Comments;
        public string Authors;
    }
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("do you want to (c)reate or (o)pen project?");
                string movestr = Console.ReadLine();
                char move;
                while (true)
                {
                    if (char.TryParse(movestr, out move))
                    {
                        break;
                    }
                    Console.Clear();
                    Console.WriteLine("do you want to (c)reate or (o)pen project?");
                    movestr = Console.ReadLine();
                }
                string dirPath = Environment.CurrentDirectory;
                if (move == 'c')
                {
                    Console.WriteLine("Enter name: ");
                    string projectName = Console.ReadLine();
                    InitNewProject(dirPath, projectName);
                }
                else
                {
                    string filePath = ProjectName(dirPath);
                    string name = filePath.Remove(0, dirPath.Length + 1);
                    Project projectObj = ReadingOneProject(filePath, name);
                    while (true)
                    {
                        Showing(projectObj);
                        HTMLexport(name, projectObj, filePath);
                        Console.WriteLine("What do you want to do? Write \"help\" for information");
                        string moving = Console.ReadLine();
                        EditingWithCommands(projectObj, moving, filePath);
                        projectObj = ReadingOneProject(filePath, name);
                    }
                }
            }
        }

        public static string ProjectName(string dirPath)
        {
            string[] filePaths = Directory.GetDirectories(dirPath);
            for (int i = 0; i < filePaths.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {filePaths[i].Remove(0, dirPath.Length + 1)}");
            }
            string choice = Console.ReadLine();
            while (true)
            {
                if (int.TryParse(choice, out int choiceint))
                {
                    return filePaths[choiceint - 1];
                }
                choice = Console.ReadLine();
            }
        }

        public static Project ReadingOneProject(string filePath, string name)
        {
            Project pr = new Project();
            pr.ProjectName = name;
            pr.Comments = "";
            pr.Done = "";
            using (StreamReader read = new StreamReader(filePath + "\\main.ctm"))
            {
                int currentReading = 0;
                while (true)
                {
                    string line = read.ReadLine();
                    if (!string.IsNullOrEmpty(line) && line[0] == '/' && line[1] == '/')
                    {
                        for (int i = 2; i < line.Length; i++)
                        {
                            pr.Comments += line[i];
                        }

                        pr.Comments += '`';
                    }

                    if (!string.IsNullOrEmpty(line))
                    {
                        switch (currentReading)
                        {
                            case 1:
                                {
                                    for (int i = 4; i < line.Length; i++)
                                    {
                                        pr.Done += line[i];
                                    }

                                    pr.Done += '`';
                                    break;
                                }

                            case 2:
                                {
                                    for (int i = 4; i < line.Length; i++)
                                    {
                                        pr.InProgress += line[i];
                                    }

                                    pr.InProgress += '`';
                                    break;
                                }

                            case 3:
                                {
                                    for (int i = 4; i < line.Length; i++)
                                    {
                                        pr.UnDone += line[i];
                                    }

                                    pr.UnDone += '`';
                                    break;
                                }
                            case 4:
                                {
                                    for (int i = 4; i < line.Length; i++)
                                    {
                                        pr.Authors += line[i];
                                    }

                                    pr.Authors += '`';
                                    break;
                                }

                        }
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == '<' && line[1] == 'd')
                    {
                        currentReading = 1;
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == '<' && line[1] == 'i')
                    {
                        currentReading = 2;
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == '<' && line[1] == 'u')
                    {
                        currentReading = 3;
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == '<' && line[1] == 'a')
                    {
                        currentReading = 4;
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == '>')
                    {
                        currentReading = 0;
                    }

                    if (!string.IsNullOrEmpty(line) && line[0] == ']')
                    {
                        break;
                    }
                }
                read.Close();
            }
            return pr;
        }

        public static void Showing(Project pr)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Project Name: {pr.ProjectName}\n");
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Authors of {pr.ProjectName}: \n");
            Console.Write("    ");
            foreach (var author in pr.Authors)
            {
                if (author != '`')
                {
                    Console.Write(author);
                }
                else
                {
                    Console.Write("\n    ");
                }
            }
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Comments to the {pr.ProjectName}: \n");
            Console.Write("    ");
            foreach (var comment in pr.Comments)
            {
                if (comment != '`')
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(comment);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n    ");
                }
            }
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"What is done in {pr.ProjectName}: \n");
            Console.Write("    ");
            foreach (var done in pr.Done)
            {
                if (done != '`')
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(done);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n    ");
                }
            }
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"What is in progress in {pr.ProjectName}: \n");
            Console.Write("    ");
            foreach (var inProgress in pr.InProgress)
            {
                if (inProgress != '`')
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(inProgress);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n    ");
                }
            }
            Console.WriteLine("");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"What is not done yet in {pr.ProjectName}: \n");
            Console.Write("    ");
            foreach (var undone in pr.UnDone)
            {
                if (undone != '`')
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(undone);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n    ");
                }
            }
            Console.Write("\n");
        }
        public static void HTMLexport(string name, Project pr, string filePath)
        {
            using (StreamWriter write = new StreamWriter(filePath + "\\index.html"))
            {
                write.WriteLine("<!DOCTYPE html>");
                write.WriteLine("<html>");
                write.WriteLine("<head>");
                write.WriteLine($"<title>{name}</title>");
                write.WriteLine("</head>");
                write.WriteLine("<body>");
                write.WriteLine($"<H1>Project name: {name}</H1>");
                write.WriteLine($"<H3 style=\"padding: 0px 0px 0px 20px;\">Authors of {name}</H3>");
                string word = "";
                foreach (var comment in pr.Authors)
                {
                    if (comment != '`')
                    {
                        word += comment;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            write.WriteLine($"<p style=\"padding: 0px 0px 0px 40px;\">{word}</p>");
                            word = "";
                        }
                    }
                }
                write.WriteLine($"<H3 style=\"padding: 0px 0px 0px 20px;\">Project Comments: {name}</H3>");
                word = "";
                foreach (var comment in pr.Comments)
                {
                    if (comment != '`')
                    {
                        word += comment;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            write.WriteLine($"<font color=\"blue\"><li style=\"padding: 0px 0px 0px 40px;\">{word}</li>");
                            word = "";
                        }
                    }
                }
                write.WriteLine($"<font color=\"black\"><H3 style=\"padding: 0px 0px 0px 20px;\">What is done in {name}</H3>");
                word = "";
                foreach (var done in pr.Done)
                {
                    if (done != '`')
                    {
                        word += done;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            write.WriteLine($"<font color=\"green\"><li style=\"padding: 0px 0px 0px 40px;\">{word}</li>");
                            word = "";
                        }
                    }
                }
                write.WriteLine($"<font color=\"black\"><H3 style=\"padding: 0px 0px 0px 20px;\">What is in Progress in {name}</H3>");
                word = "";
                foreach (var inProgress in pr.InProgress)
                {
                    if (inProgress != '`')
                    {
                        word += inProgress;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            write.WriteLine($"<font color=\"#ff9900\"><li style=\"padding: 0px 0px 0px 40px;\">{word}</li>");
                            word = "";
                        }
                    }
                }
                write.WriteLine($"<font color=\"black\"><H3 style=\"padding: 0px 0px 0px 20px;\">What is not done in {name}</H3>");
                word = "";
                foreach (var undone in pr.UnDone)
                {
                    if (undone != '`')
                    {
                        word += undone;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(word))
                        {
                            write.WriteLine($"<font color=\"red\"><li style=\"padding: 0px 0px 0px 40px;\">{word}</li>");
                            word = "";
                        }
                    }
                }
                write.WriteLine("</body>");
                write.WriteLine("</html>");
                write.Close();
            }
        }
        public static void InitNewProject(string dirPath, string name)
        {
            string prjPath = dirPath + "\\" + name;
            Directory.CreateDirectory(prjPath);
            using (StreamWriter writer = File.CreateText(prjPath + "\\main.ctm"))
            {
                writer.WriteLine($"[{name}");
                writer.WriteLine("<a");
                Console.WriteLine($"Authors of {name}? Enter \"all\" for finishing");
                string authors = Console.ReadLine();
                while (authors != "all")
                {
                    writer.WriteLine($"    {authors}");
                    authors = Console.ReadLine();
                }
                writer.WriteLine(">");
                writer.WriteLine("<d");
                Console.WriteLine($"What you have already done in {name}? Enter \"all\" for finishing");
                string done = Console.ReadLine();
                while (done != "all")
                {
                    writer.WriteLine($"    {done}");
                    done = Console.ReadLine();
                }
                writer.WriteLine(">");
                writer.WriteLine("<i");
                Console.WriteLine($"What you have in progress in {name}? Enter \"all\" for finishing");
                string inPrgrs = Console.ReadLine();
                while (inPrgrs != "all")
                {
                    writer.WriteLine($"    {inPrgrs}");
                    inPrgrs = Console.ReadLine();
                }
                writer.WriteLine(">");
                writer.WriteLine("<u");
                Console.WriteLine($"What you haven't done yet in {name}? Enter \"all\" for finishing");
                string undone = Console.ReadLine();
                while (undone != "all")
                {
                    writer.WriteLine($"    {undone}");
                    undone = Console.ReadLine();
                }
                writer.WriteLine(">");
                writer.WriteLine("]");
                writer.Close();
            }
        }

        public static void EditingWithCommands(Project pr, string move, string filePath)
        {
            char separator = '`';
            string curMove = "";
            int i = 0;
            while (i < move.Length && move[i] != ' ')
            {
                curMove += move[i];
                i++;
            }
            i++;
            switch (curMove)
            {
                case "help":
                    {
                        Console.Clear();
                        Console.WriteLine("Enter \"new PLACE* SOMETHING\" to create new task in one of the PLACES");
                        Console.WriteLine("Enter \"delete PLACE SOMETHING\" to delete task in one of the PLACES");
                        Console.WriteLine("Enter \"move PLACE1 PLACE2 SOMETHING\" to move task from PLACE1 to PLACE2\n");
                        Console.WriteLine("*PLACE:");
                        Console.WriteLine("    comment");
                        Console.WriteLine("    done");
                        Console.WriteLine("    inprogress");
                        Console.WriteLine("    undone");
                        Console.WriteLine("    author");
                        Console.ReadKey();
                        break;
                    }
                case "delete":
                    {
                        bool deleted = false;
                        string dir = "";
                        while (move[i] != ' ')
                        {
                            dir += move[i];
                            i++;
                        }
                        i++;
                        dir = dir.ToLower();
                        string smth = "";
                        while (i < move.Length)
                        {
                            smth += move[i];
                            i++;
                        }
                        switch (dir)
                        {
                            case "author":
                            {
                                string[] list = pr.Authors.Split(separator);
                                string newStr = "";
                                foreach (var listunit in list)
                                {
                                    if (!string.IsNullOrEmpty(listunit))
                                    {
                                        if (listunit == smth && !deleted)
                                        {
                                            deleted = true;
                                        }
                                        else
                                        {
                                            newStr += listunit + '`';
                                        }
                                    }
                                }

                                pr.Authors = newStr;
                                break;
                            }
                            case "comment":
                                {
                                    string[] list = pr.Comments.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }

                                    pr.Comments = newStr;
                                    break;
                                }
                            case "done":
                                {
                                    string[] list = pr.Done.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.Done = newStr;
                                    break;
                                }

                            case "inprogress":
                                {
                                    string[] list = pr.InProgress.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.InProgress = newStr;
                                    break;
                                }

                            case "undone":
                                {
                                    string[] list = pr.UnDone.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.UnDone = newStr;
                                    break;
                                }
                        }
                        break;
                    }
                case "new":
                    {
                        string dir = "";
                        while (move[i] != ' ')
                        {
                            dir += move[i];
                            i++;
                        }
                        i++;
                        dir = dir.ToLower();
                        string smth = "";
                        while (i < move.Length)
                        {
                            smth += move[i];
                            i++;
                        }
                        switch (dir)
                        {
                            case "author":
                                {
                                    pr.Authors += smth;
                                    break;
                                }
                            case "comment":
                                {
                                    pr.Comments += smth;
                                    break;
                                }
                            case "done":
                                {
                                    pr.Done += smth;
                                    break;
                                }

                            case "inprogress":
                                {
                                    pr.InProgress += smth;
                                    break;
                                }

                            case "undone":
                                {
                                    pr.InProgress += smth;
                                    break;
                                }
                        }
                        break;
                    }
                case "move":
                    {
                        bool deleted = false;
                        string dir1 = "";
                        while (move[i] != ' ')
                        {
                            dir1 += move[i];
                            i++;
                        }
                        i++;
                        string dir2 = "";
                        while (move[i] != ' ')
                        {
                            dir2 += move[i];
                            i++;
                        }
                        i++;
                        dir1 = dir1.ToLower();
                        dir2 = dir2.ToLower();
                        string smth = "";
                        while (i < move.Length)
                        {
                            smth += move[i];
                            i++;
                        }
                        switch (dir1)
                        {
                            case "author":
                                {
                                    string[] list = pr.Authors.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }

                                    pr.Authors = newStr;
                                    break;
                                }
                            case "comment":
                                {
                                    string[] list = pr.Comments.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }

                                    pr.Comments = newStr;
                                    break;
                                }
                            case "done":
                                {
                                    string[] list = pr.Done.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.Done = newStr;
                                    break;
                                }

                            case "inprogress":
                                {
                                    string[] list = pr.InProgress.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.InProgress = newStr;
                                    break;
                                }

                            case "undone":
                                {
                                    string[] list = pr.UnDone.Split(separator);
                                    string newStr = "";
                                    foreach (var listunit in list)
                                    {
                                        if (!string.IsNullOrEmpty(listunit))
                                        {
                                            if (listunit == smth && !deleted)
                                            {
                                                deleted = true;
                                            }
                                            else
                                            {
                                                newStr += listunit + '`';
                                            }
                                        }
                                    }
                                    pr.UnDone = newStr;
                                    break;
                                }
                        }
                        switch (dir2)
                        {
                            case "author":
                                {
                                    pr.Authors += smth;
                                    break;
                                }
                            case "comment":
                                {
                                    pr.Comments += smth;
                                    break;
                                }
                            case "done":
                                {
                                    pr.Done += smth;
                                    break;
                                }

                            case "inprogress":
                                {
                                    pr.InProgress += smth;
                                    break;
                                }

                            case "undone":
                                {
                                    pr.UnDone += smth;
                                    break;
                                }
                        }
                        break;
                    }
            }
            FileFromPr(filePath, pr);
        }
        public static void FileFromPr(string filePath, Project pr)
        {
            File.Delete(filePath + "\\main.ctm");
            using (StreamWriter writer = File.CreateText(filePath + "\\main.ctm"))
            {
                writer.WriteLine('[' + pr.ProjectName);
                if (!string.IsNullOrEmpty(pr.Comments))
                {
                    char separator = '`';
                    string[] list = pr.Comments.Split(separator);
                    foreach (var comment in list)
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            writer.Write("//");
                            writer.WriteLine(comment);
                        }
                    }
                }
                writer.WriteLine("<a");
                if (!string.IsNullOrEmpty(pr.Done))
                {
                    char separator = '`';
                    string[] list = pr.Authors.Split(separator);
                    foreach (var comment in list)
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            writer.Write("    ");
                            writer.WriteLine(comment);
                        }
                    }
                }
                writer.WriteLine("<d");
                if (!string.IsNullOrEmpty(pr.Done))
                {
                    char separator = '`';
                    string[] list = pr.Done.Split(separator);
                    foreach (var comment in list)
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            writer.Write("    ");
                            writer.WriteLine(comment);
                        }
                    }
                }
                writer.WriteLine(">");
                writer.WriteLine("<i");
                if (!string.IsNullOrEmpty(pr.InProgress))
                {
                    char separator = '`';
                    string[] list = pr.InProgress.Split(separator);
                    foreach (var comment in list)
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            writer.Write("    ");
                            writer.WriteLine(comment);
                        }
                    }
                }
                writer.WriteLine(">");
                writer.WriteLine("<u");
                if (!string.IsNullOrEmpty(pr.UnDone))
                {
                    char separator = '`';
                    string[] list = pr.UnDone.Split(separator);
                    foreach (var comment in list)
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            writer.Write("    ");
                            writer.WriteLine(comment);
                        }
                    }
                }
                writer.WriteLine(">");
                writer.WriteLine("]");
            }
        }
    }
}
