﻿using System;
using System.Windows.Forms;
using System.Net;


namespace CanvasAPIApp
{
    public partial class AccessTokenForm : Form
    {
        GeneralAPIGets getProfile = new GeneralAPIGets();

        public AccessTokenForm()
        {
            InitializeComponent();
        }

        private void AccessTokenForm_Load(object sender, EventArgs e)
        {

        }//End Load

        //Saving token
        private void saveAccessToken_Click(object sender, EventArgs e)
        {
            //Saving User input


            //Setting variable for try catch and web call
            String currentAccessToken = Properties.Settings.Default.CurrentAccessToken;


            //Set wait cursor
            Cursor.Current = Cursors.WaitCursor;
            //Make Web call to get name
            if (txbCurrentAccessToken.Text != "No Access Token")
            {

                try
                {

                    //Setting new access token


                    Properties.Settings.Default.CurrentAccessToken = txbCurrentAccessToken.Text.ToString();
                    Properties.Settings.Default.Save();

                    //REST object to get
                    string name = "name";
                    //Display Name of current profile this is an API Call
                    lbxCurrentUser.Text = getProfile.GetProfile(name);
                    //Save name to the app settings
                    Properties.Settings.Default.AppUserName = lbxCurrentUser.Text;
                    Properties.Settings.Default.Save();
                }
                catch (Exception callException)
                {
                    //set token text box
                    txbCurrentAccessToken.Text = "No Access Token";
                    //set no user in user text box
                    lbxCurrentUser.Text = "No User Loaded";
                    //save no access token to settings
                    Properties.Settings.Default.CurrentAccessToken = "No Access Token";
                    Properties.Settings.Default.Save();
                    //Change option on close/cancel button
                    btnCancel.Text = "Close";
                    //create strong from exception
                    string callExceptionString = callException.ToString();

                    if (callExceptionString.Contains("401"))
                        MessageBox.Show("Access Denied! Please Check Access Token and Website Name. \n\n" + callException.ToString());
                    else
                        MessageBox.Show(callException.ToString());
                }
                //Change the cancel button to close
                btnCancel.Text = "Close";
            }
            else
            {
                txbCurrentAccessToken.Text = "No Access Token";
                lbxCurrentUser.Text = "No User Loaded";
                btnCancel.Text = "Close";
                Properties.Settings.Default.AppUserName = lbxCurrentUser.Text;
                Properties.Settings.Default.CurrentAccessToken = "No Access Token";
                Properties.Settings.Default.Save();
            }

            PopulateSavedSettings();

            Cursor.Current = Cursors.Default;
        }//end saving token

        //Saving Website
        private void btnSaveWebsite_Click(object sender, EventArgs e)
        {

            WebRequest webRequest = WebRequest.Create(txbWebsite.Text.ToString());
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
                MessageBox.Show("The site has been saved.", "Success", MessageBoxButtons.OK);
            }
            //if HttpWebResponse response = (HttpWebResponse)request.GetResponse(); fails then throw an error
            catch (WebException)
            {
                MessageBox.Show("Failed to reach site.", "Failed", MessageBoxButtons.OK);
            }
            //Saving Website to settings
            Properties.Settings.Default.InstructureSite = txbWebsite.Text.ToString();
            Properties.Settings.Default.Save();
            btnCancel.Text = "Close";
            
        }//End Saving website

        private void PopulateSavedSettings()
        {
            //Displaying current settings
            txbWebsite.Text = Properties.Settings.Default.InstructureSite.ToString();
            string currentAccessToken = Properties.Settings.Default.CurrentAccessToken;
            //Display new access Token with mask make sure the sting is long enough
            if (currentAccessToken.Length > 7)
            {
                txbCurrentAccessToken.Text = (string.Concat(currentAccessToken.Substring(0, 7), "".PadRight(currentAccessToken.Length - 7, '*')));
            }

        }

        //Closing Form
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); //closes this form, but continues in CanvasAPIMainForm

        }//End Closing Form

        private void AccessTokenForm_Shown(object sender, EventArgs e)
        {

            if (Properties.Settings.Default.CurrentAccessToken != "No Access Token")
            {

                try
                {
                    string name = "name";
                    lbxCurrentUser.Text = getProfile.GetProfile(name);

                }
                catch (Exception callException)
                {
                    string callExceptionString = callException.ToString();
                    if (callExceptionString.Contains("401"))
                        MessageBox.Show("Access Denied! Please Check Access Token and Website Name. \n\n" + callException.ToString());
                    else
                        MessageBox.Show(callException.ToString());
                }

            }
            PopulateSavedSettings();
        }
    }
}
