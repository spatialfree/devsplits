using System;
using System.Threading;

public class Program {
  class Task {
    public string name;
    public float alloted, elapsed;

    public Task(string name, float alloted) {
      this.name = name;
      this.alloted = alloted;
      this.elapsed = 0;
    }
  }

  public static void Main() {
    Task[] tasks = new Task[5];
    Console.WriteLine("welcome to devtime!"); // mention an aggragate stat

    while (true) {
      Console.WriteLine("");
      Console.Write("command: ");
      string input = Console.ReadLine();
      Console.Clear();
      string[] words = input.Split(' ');
      string command = words[0];
      switch (words[0]) {
        case "help":
          Console.WriteLine("add");
          Console.WriteLine("list");
          Console.WriteLine("remove <task>");
          Console.WriteLine("doing <task>");
          Console.WriteLine("complete");
          break;
        case "add":
          Add();
          break;
        case "list":
          List();
          break;
        case "remove":
          Remove(int.Parse(words[1]));
          break;
        case "doing":
          Doing(int.Parse(words[1]));
          break;
        case "complete":
          Complete();
          break;
        default:
          Console.WriteLine("invalid command");
          break;
      }
    }

    void Add() {
      Console.Write("what is your task? ");
      string taskName = Console.ReadLine();
      Console.Write("how many minutes do you need? ");
      float taskTime = float.Parse(Console.ReadLine());
      Task task = new Task(taskName, taskTime);
      for (int i = 0; i < tasks.Length - 1; i++) {
        if (tasks[i] == null) {
          tasks[i] = task;
          return;
        }
      }
      Console.WriteLine("no more tasks can be added");
    }

    void List() {
      float total = 0;
      for (int i = 0; i < tasks.Length; i++) {
        Task task = tasks[i];
        if (task != null) {
          Console.WriteLine($"[{i}] {task.name} | alloted:{ToTime(task.alloted)} elapsed:{ToTime(task.elapsed)}");
          total += task.alloted;
        }
      }

      Console.WriteLine($"total alloted: {total}");
    }

    void Remove(int index) {
      if (index < 0 || index >= tasks.Length || tasks[index] == null) {
        Console.WriteLine("invalid index");
        return;
      }
      tasks[index] = null;
    }

    void Doing(int index) {
      Task task = tasks[index];
      if (task == null) {
        Console.WriteLine("invalid index");
        return;
      }

      bool running = true;
      while (running) {
        Thread.Sleep(1000);
        Console.Clear();
        task.elapsed += 1f / 60;
        Console.WriteLine($"{task.name} | alloted:{ToTime(task.alloted)} elapsed:{ToTime(task.elapsed)}");

        if (task.elapsed >= task.alloted) {
          Console.Beep();
        }

        running = !Console.KeyAvailable;
      }
  
      Console.WriteLine($"delta: {ToTime(task.alloted - task.elapsed)}");
      Console.WriteLine("...don't forgit to git commit(s)");
    }

    void Complete() {
      // list and then show totals
      List();

      float delta = 0;
      for (int i = 0; i < tasks.Length; i++) {
        Task task = tasks[i];
        if (task != null) {
          delta += task.alloted - task.elapsed;
        }
      }
      Console.WriteLine($"total delta: {ToTime(delta)}");
      // encourages not undercutting the alloted time for the necessary work
      // if delta is positive then you can decide what to do with it
      Console.Write("(f)ree or (s)ide project with this delta? ");
      string input = Console.ReadLine();
      if (input == "s") {
        tasks[tasks.Length - 1] = new Task("side project", delta);
        Doing(tasks.Length - 1);
      } else {
        Console.WriteLine("time flies");
      }
            

      // snapshot and log to file when completed // backup in repo!
      // current DATE


      Console.WriteLine("...don't forgit to push commit(s)");


      for (int i = 0; i < tasks.Length; i++) { tasks[i] = null; }
    }


    // int n = int.Parse(Console.ReadLine());
  }

  public static string ToTime(float t) {
    // account for negative t
    if (t < 0) {
      return $"-{ToTime(-t)}";
    }

    float minutes = t * 60;
    var ss = Convert.ToInt32(minutes % 60).ToString("00");
    var mm = (Math.Floor(minutes / 60) % 60).ToString("00");
    var hh = Math.Floor(minutes / 60 / 60).ToString("00");
    // return TimeSpan.FromSeconds(t).ToString("HH':'mm':'ss");
    return $"{hh}:{mm}:{ss}";
  }
}