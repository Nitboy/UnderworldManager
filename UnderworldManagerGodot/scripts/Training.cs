using Godot;
using System;
using UnderworldManager.Core.Models;
using UnderworldManager.Business;

public partial class Training : Control
{
    private Character playerCharacter;
    private RichTextLabel characterInfo;
    private RichTextLabel trainingLog;
    private Button[] trainingButtons;

    public override void _Ready()
    {
        // Get references to UI elements
        characterInfo = GetNode<RichTextLabel>("VBoxContainer/CharacterInfo");
        trainingLog = GetNode<RichTextLabel>("VBoxContainer/TrainingLog");
        
        // Get training buttons
        trainingButtons = new Button[4];
        trainingButtons[0] = GetNode<Button>("VBoxContainer/TrainingOptions/StrengthButton");
        trainingButtons[1] = GetNode<Button>("VBoxContainer/TrainingOptions/AgilityButton");
        trainingButtons[2] = GetNode<Button>("VBoxContainer/TrainingOptions/IntelligenceButton");
        trainingButtons[3] = GetNode<Button>("VBoxContainer/TrainingOptions/CharismaButton");

        // Connect button signals
        foreach (var button in trainingButtons)
        {
            button.Pressed += () => OnTrainingButtonPressed(button);
        }

        // Connect back button
        GetNode<Button>("VBoxContainer/BackButton").Pressed += OnBackButtonPressed;

        // Get player character from main scene
        var mainScene = GetTree().Root.GetNode<Main>("Main");
        playerCharacter = mainScene.PlayerCharacter;

        // Update UI
        UpdateCharacterInfo();
    }

    private void UpdateCharacterInfo()
    {
        characterInfo.Text = $"[b]Character Information[/b]\n" +
                           $"Name: {playerCharacter.Name}\n" +
                           $"Profession: {playerCharacter.Profession}\n" +
                           $"Rating: {playerCharacter.Rating}\n\n" +
                           $"[b]Attributes[/b]\n" +
                           $"Strength: {playerCharacter.Strength}\n" +
                           $"Agility: {playerCharacter.Agility}\n" +
                           $"Intelligence: {playerCharacter.Intelligence}\n" +
                           $"Charisma: {playerCharacter.Charisma}";
    }

    private void OnTrainingButtonPressed(Button button)
    {
        string attribute = button.Text.Split(' ')[0];
        int currentValue = 0;
        int newValue = 0;

        switch (attribute)
        {
            case "Strength":
                currentValue = playerCharacter.Strength;
                playerCharacter.Strength++;
                newValue = playerCharacter.Strength;
                break;
            case "Agility":
                currentValue = playerCharacter.Agility;
                playerCharacter.Agility++;
                newValue = playerCharacter.Agility;
                break;
            case "Intelligence":
                currentValue = playerCharacter.Intelligence;
                playerCharacter.Intelligence++;
                newValue = playerCharacter.Intelligence;
                break;
            case "Charisma":
                currentValue = playerCharacter.Charisma;
                playerCharacter.Charisma++;
                newValue = playerCharacter.Charisma;
                break;
        }

        // Update training log
        trainingLog.Text += $"\nTrained {attribute}: {currentValue} -> {newValue}";

        // Update character info
        UpdateCharacterInfo();
    }

    private void OnBackButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed)
            {
                switch (eventKey.Keycode)
                {
                    case Key.S:
                        GetNode<Button>("VBoxContainer/TrainingOptions/StrengthButton").EmitSignal("pressed");
                        break;
                    case Key.A:
                        GetNode<Button>("VBoxContainer/TrainingOptions/AgilityButton").EmitSignal("pressed");
                        break;
                    case Key.I:
                        GetNode<Button>("VBoxContainer/TrainingOptions/IntelligenceButton").EmitSignal("pressed");
                        break;
                    case Key.C:
                        GetNode<Button>("VBoxContainer/TrainingOptions/CharismaButton").EmitSignal("pressed");
                        break;
                    case Key.B:
                        OnBackButtonPressed();
                        break;
                }
            }
        }
    }
} 