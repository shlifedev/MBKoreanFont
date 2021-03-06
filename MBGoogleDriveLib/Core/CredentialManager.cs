﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

public static class CredentialManager
{
    
    static string ApplicationName = "MB Translate";
    public static Google.Apis.Drive.v3.DriveService DriveService;
    public static Google.Apis.Sheets.v4.SheetsService SheetService;
    private static string credentialPath = "secret.json";
    public static void InitCredentialManager(string secretPath)
    {
        credentialPath = secretPath;
    } 
    public static void CredentialDriveServiceByToken()
    {
        if (DriveService == null)
        {
            UserCredential credential;
            using (var stream =
                new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new string[] { DriveService.Scope.DriveReadonly },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            DriveService = service;
            DriveManager.Init(service);
        }
    }
    public static void CredentialSheetServiceByToken()
    {
        if (DriveService == null)
        {
            UserCredential credential;
            using (var stream =
                new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new string[] { SheetsService.Scope.Spreadsheets },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            SheetService = service;
            SheetManager.Init(service);
        }
    }
}