﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Anachrophobe
{
    public partial class uxContainerForm : Form
    {
        // m_ScreenBounds is a container for a magic number that should be replaced programatically in the future.
        Point m_ScreenBounds;
        ContainerObject m_Container = new ContainerObject();
        bool deSerializerActive = true;

        public uxContainerForm()
        {
            InitializeComponent();
        }

        private void SaveControls()
        {
            //XmlFormatter xf = new XmlFormatter(typeof(ContainerObject));
            BinaryFormatter bf = new BinaryFormatter();
            //XmlSerializer xf = new XmlSerializer(typeof(ContainerObject));
            FileStream fs = new FileStream("state.dat", FileMode.OpenOrCreate);
            //xf.Serialize(fs, m_Container);
            //xf.Serialize(fs, m_Container);
            bf.Serialize(fs, m_Container);
            fs.Close();
        }

        private void LoadControls()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream("state.dat", FileMode.Open);

                BinaryFormatter bf = new BinaryFormatter();
                //XmlFormatter xf = new XmlFormatter(typeof(ContainerObject));

                m_Container = (ContainerObject)bf.Deserialize(fs);

                for (int i = 0; i < m_Container.Objects.Count(); i++)
                {
                    actionClockControl acc = new actionClockControl(m_Container.getObject(i));
                    uxFlowPanel.Controls.Add(acc);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Couldn't find file");
            }
            catch
            {
                MessageBox.Show("Couldn't parse file");
            }
            finally
            {
                if(fs != null)
                    fs.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EventMessenger.Changed += EventMessenger_Changed;

            try
            {
                LoadControls();
            }
            catch
            {
                MessageBox.Show("Whoopsie");
            }
            // StringBuilder is a more efficient way of creating strings than concatination.
            StringBuilder licenseText = new StringBuilder();
            licenseText.Append("Standard GPL License");
            uxLicenseText.Text = licenseText.ToString();

            // We need to know how big the screen is, this application is always full screen
            Height = Screen.PrimaryScreen.WorkingArea.Height;
            Width = Screen.PrimaryScreen.WorkingArea.Width;

            // Here we generate the magic number based on the size of the window.
            m_ScreenBounds.X = this.Height - 281;
            m_ScreenBounds.Y = this.Width - 893;

            

            // last thing to do
            deSerializerActive = false;
        }

        void EventMessenger_Changed(object sender, ActionObject e, bool add, bool del)
        {
            if (deSerializerActive == false)
            {
                if (add == true && del == false)
                {
                    m_Container.addObject(e);
                    SaveControls();
                    MessageBox.Show("saved");
                }
                if (add == false && del == true)
                {
                    m_Container.delObject(e);
                }
            }
        }

        private void uxKillProgram_Click(object sender, EventArgs e)
        {
            // Do any cleanup in this function
            this.Dispose();
        }

        private void uxAddTimer_Click(object sender, EventArgs e)
        {
            // The try/catches are an easier way to avoid stupid crashes, and give the user information to give to the developer
            try
            {
                if (uxFlowPanel.Controls.Count == 0)
                {
                    if (uxYUPmode.Checked == true)
                    {
                        // The final true in this function is for telling the control it is to re-start itself with a new date one week after the first
                        // this parameter is optional.
                        // Make a new actionClockControl
                        actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text, true);
                        // Put the control into the FlowPanel
                        scc.Parent = uxFlowPanel;
                        // Display the control
                        scc.Show();
                    }
                    else
                    {
                        // Make a normal actionClockControl
                        actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text);
                        scc.Parent = uxFlowPanel;
                        scc.Show();
                    }
                }
                else
                {
                    actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text);
                    scc.Parent = uxFlowPanel;
                    scc.Show();
                }
                // Blank all the input parameters and set the focus to the first one.
                // This makes it easier to begin typing the next set of parameters
                uxTimeOfAction.Text = "";
                uxNameOfAction.Text = "";
                uxLengthOfAction.Text = "";
                uxTimeOfAction.Focus();
            }
            catch
            {
                // Errors should be formatted like this, "Error in <filename>, <method>"
                MessageBox.Show("Error in uxContainerForm, uxAddTimer_Click");
            }

        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Update the time in the large textbox, pretty simple.
                uxCurrentTime.Text = DateTime.Now.ToString();
            }
            catch
            {
                // We should NEVER encounter this, but just in case.
                MessageBox.Show("Error in uxContainerForm, ClockTimer_Tick");
            }
        }

        private void uxTimeOfAction_TextChanged(object sender, EventArgs e)
        {
            // As soon as the DateTime of uxTimeOfAction is filled out, switch to uxLengthOfAction
            // This avoids the need to tab to the next control
            if (uxTimeOfAction.MaskFull)
            {
                uxLengthOfAction.Focus();
            }
        }

        private void uxLengthOfAction_TextChanged(object sender, EventArgs e)
        {
            // Switch to uxNameOfAction after uxLengthOfAction is completed
            if (uxLengthOfAction.MaskFull)
            {
                uxNameOfAction.Focus();
            }
        }

        void uxNameOfAction_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // KeyValue 13 is the enter button
            // We want to treat an enter key event as pressing the add timer button.
            // Possibly replace the code after this if with a function to reduce code size
            if (e.KeyValue == 13)
            {
                try
                {
                    if (uxFlowPanel.Controls.Count == 0)
                    {
                        if (uxYUPmode.Checked == true)
                        {
                            actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text, true);
                            scc.Parent = uxFlowPanel;
                            scc.Show();
                        }
                        else
                        {
                            actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text);
                            scc.Parent = uxFlowPanel;
                            scc.Show();
                        }
                    }
                    else
                    {
                        actionClockControl scc = new actionClockControl(uxTimeOfAction.Text, uxLengthOfAction.Text, uxNameOfAction.Text);
                        scc.Parent = uxFlowPanel;
                        scc.Show();
                    }
                    // Clear all the data parameters and prepare for the next entry
                    uxTimeOfAction.Text = "";
                    uxNameOfAction.Text = "";
                    uxLengthOfAction.Text = "";
                    uxTimeOfAction.Focus();
                }
                catch
                {
                    MessageBox.Show("Error in uxContainerForm, uxNameOfAction_KeyDown");
                }
            }
            else
            {
                //e.Handled = false;
            }
        }
    }
}
