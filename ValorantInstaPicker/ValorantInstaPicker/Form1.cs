using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ValorantInstaPicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        static string agentsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agents.json");
        static List<Agent> agents = new List<Agent>();
        public Form1()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Interval = 100;
        }

        int cum = 0;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if(cum == 0)
            {
                timer.Start();
                cum = 1;
            }
            else
            {
                timer.Stop();
                cum = 0;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            // Convert screen coordinates to client coordinates
            Point mousePosition = Cursor.Position;

            // Update the label text
            label1.Text = $"Mouse position: ({mousePosition.X}, {mousePosition.Y})";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(agentsFilePath))
            {
                // Load the agents.json file
                string json = File.ReadAllText(agentsFilePath);
                agents = JsonConvert.DeserializeObject<List<Agent>>(json);
                foreach (Agent agent in agents)
                {
                    comboBox1.Items.Add(agent.name);
                }

            }
            else
            {
                // Create a new agents.json file with a single agent named "test" with x and y values of 100
                Agent testAgent = new Agent() { name = "test", x = 100, y = 100 };
                agents.Add(testAgent);
                string json = JsonConvert.SerializeObject(agents);
                File.WriteAllText(agentsFilePath, json);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int x = Convert.ToInt32(numericUpDown4.Value);
            int y = Convert.ToInt32(numericUpDown3.Value);
            Agent myAgent = new Agent() { name = textBox1.Text, x = x, y = y };
            agents.Add(myAgent);
            string agentsJson = JsonConvert.SerializeObject(agents);
            File.WriteAllText(agentsFilePath, agentsJson);
        }
        class Agent
        {
            public string name { get; set; }
            public int x { get; set; }
            public int y { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string json = File.ReadAllText(agentsFilePath);
            comboBox1.Items.Clear();
            agents = JsonConvert.DeserializeObject<List<Agent>>(json);
            foreach (Agent agent in agents)
            {
                comboBox1.Items.Add(agent.name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                // No agent is selected
                MessageBox.Show("Please select an agent");
                return;
            }

            // Get the selected agent
            string selectedAgentName = comboBox1.SelectedItem.ToString();
            Agent selectedAgent = agents.FirstOrDefault(a => a.name == selectedAgentName);

            // Check if the agent was found
            if (selectedAgent == null)
            {
                MessageBox.Show("Selected agent not found");
                return;
            }

            // Write the x and y values to the console
            MessageBox.Show($"X: {selectedAgent.x}, Y: {selectedAgent.y}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                // No agent is selected
                MessageBox.Show("Please select an agent");
                return;
            }
            string selectedAgentName = comboBox1.SelectedItem.ToString();
            Agent selectedAgent = agents.FirstOrDefault(a => a.name == selectedAgentName);

            if (selectedAgent == null)
            {
                MessageBox.Show("Selected agent not found");
                return;
            }

            var startTime = DateTime.UtcNow;

            while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(10))
            {
                // Set the cursor position
                Cursor.Position = new Point(selectedAgent.x, selectedAgent.y);

                // Perform a left click
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                System.Threading.Thread.Sleep(1);
                // Set the cursor position
                int x = Convert.ToInt32(numericUpDown6.Value);
                int y = Convert.ToInt32(numericUpDown5.Value);
                Cursor.Position = new Point(x, y);

                // Perform a left click
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }

            
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.L)
            {
                button5.PerformClick();
            }
        }
    }
}
