using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HeroApp; 

public partial class HeroDb : Form {
    public readonly FileSystemWatcher Fsw;

    private static readonly string StartupPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;

    public HeroDb()
    {
        InitializeComponent();
        Fsw = new FileSystemWatcher(StartupPath)
        {
            EnableRaisingEvents = true,
            Filter = "generated*.json",
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
        };
        Fsw.Changed += FswOnChanged;
        Fsw.Created += FswOnChanged;
        Fsw.Deleted += FswOnChanged;
        Fsw.Renamed += FswOnChanged;
        BuildList();
    }

    private void FswOnChanged(object sender, FileSystemEventArgs e) {
        if (InvokeRequired)
        {
            Invoke(BuildList);
        }
        else
        {
            BuildList();
        }
    }

    private void BuildList() {
        listBox1.Items.Clear();
        string[] files = Directory.GetFiles(StartupPath, "generated*.json");
        foreach (string file in files) {
            try {

                var hero = JsonConvert.DeserializeObject< Hero >(File.ReadAllText(file));
                listBox1.Items.Add(hero!);
            } catch (Exception)
            {
                // ignored. If it fails to parse, just move on.
            }
        }
    }

    private void HeroDb_Load(object sender, EventArgs e)
    {
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        textBox1.Text = (listBox1.SelectedItem as Hero)!.Display();
    }

    private void button1_Click(object sender, EventArgs e) {
        Close();
    }
}