using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RemoteDesktopLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ListOfNodes = new List<String>();
            InitializeComponent();

            loadList(NodeXML);
        }

        private string NodeXML = "RDPLauncher.xml";


        private void newNode_TextChanged(object sender, TextChangedEventArgs e)
        {
        }



        private void newNode_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.newNode.Text = "";
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var nodecheck = Regex.Match(this.newNode.Text, "$^|NodeName");
            
            if (nodecheck.Success)
            {
                this.notification.Text = "enter a valid node name.";
            } 
            else
            {
                this.ListOfNodes.Add(this.newNode.Text);
                this.newNode.Text = "NodeName";
                this.notification.Text = "";
                updateListbox();
                saveList(NodeXML);
            }

        }

        private List<String> ListOfNodes { get; set; }

        private void updateListbox()
        {
            this.nodeList.Items.Clear();

            foreach (string node in this.ListOfNodes)
            {
                this.nodeList.Items.Add(node);
            }
            
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            string item = nodeList.SelectedValue.ToString();
            
            ListOfNodes.Remove(ListOfNodes.Where(i => i == item).First());

            updateListbox();
            saveList(NodeXML);

        }

        private void launchRDP(string hostname)
        {
            string processArgs = String.Format("/v {0}", hostname);
            System.Diagnostics.Process.Start("mstsc.exe", processArgs);

        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {

            string node = nodeList.SelectedValue.ToString();
            launchRDP(node);
        }

        private void saveList(string filePath)
        {
            XElement xmlElements = new XElement("nodes", this.ListOfNodes.Select(i => new XElement("node", i)));
            xmlElements.Save(filePath);
        }

        private void loadList(string filePath)
        {

            XElement xmlElements = XElement.Load(filePath);

            foreach (var xmlObject in xmlElements.Descendants("node"))    
            {
                this.ListOfNodes.Add(xmlObject.Value);
            }

            updateListbox();
        }

    }
}
