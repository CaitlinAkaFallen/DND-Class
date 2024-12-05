using System;
using System.Net.Http;
using System.IO; // For Path.Combine

public class CPHInline
{
    private static readonly HttpClient client = new HttpClient();

    // Assuming rawInput can be assigned from outside or another part of your program
    private string rawInput;

    public static HttpClient Client => client;

    public string RawInput
    {
        get => rawInput;
        set => rawInput = value;
    }

    public bool Execute()
    {
        // D&D classes available in Baldur's Gate 3
        string[] classes = { "Barbarian", "Bard", "Cleric", "Druid", "Fighter", "Monk", "Paladin", "Ranger", "Rogue", "Sorcerer", "Warlock", "Wizard" };

        // The folder path where images are stored
        string imageFolderPath = @"C:\Users\caitl\OneDrive\Desktop\DNDBG3Class"; // Change this path to the actual folder location

        // Get the Command ID, target user, and Image file path for the DND/BG3 Class
        if (!CPH.TryGetArg("commandId", out Guid commandId) || 
            !CPH.TryGetArg("user", out string user) || 
            !CPH.TryGetArg("rawInput", out rawInput))
        {
            CPH.LogError("No input provided.");
            return false;
        }

        // Process user input and check if it matches a valid class
        string selectedClass = rawInput.Trim();
        bool isValidClass = false;

        foreach (string className in classes)
        {
            if (string.Equals(selectedClass, className, StringComparison.OrdinalIgnoreCase))
            {
                isValidClass = true;
                CPH.LogInfo($"You have selected the class: {selectedClass}");

                // Combine the image folder path with the selected class image file name
                string imageFileName = $"{selectedClass.ToLower()}.png"; // Assuming image files are named after the class in lowercase
                string imagePath = Path.Combine(imageFolderPath, imageFileName);

                CPH.LogInfo($"Image file path for {selectedClass}: {imagePath}");

                // Set the image in OBS
                string scene = "Game Scene"; // Replace with the actual scene name
                string source = "DND Class Image"; // Replace with the actual source name
                CPH.ObsSetImageSourceFile(scene, source, imagePath);

                // Send DND Class message to the chat
                CPH.SendMessage($"@{user} selected the class: {selectedClass}");
                CPH.LogInfo($"Message sent to chat: @{user} selected the class: {selectedClass}");

                break;
            }
        }

        if (!isValidClass)
        {
            CPH.LogError("Invalid class selection. Please choose a valid D&D class from the list.");
            // Optional: Send a message to the user about the invalid selection
            CPH.SendMessage($"@{user} invalid class selection. Please choose from: {string.Join(", ", classes)}");
            return false;
        }

        return true;
    }
}