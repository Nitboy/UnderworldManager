using Godot;
using System;
using System.Text;
using UnderworldManager.Core.Models;
using UnderworldManager.Business;

public partial class CharacterDetails : Control
{
    private Character playerCharacter;
    private RichTextLabel characterInfo;
    private Label[] statValues;
    private Label skillsValue;

    public override void _Ready()
    {
        // Get references to UI elements
        characterInfo = GetNode<RichTextLabel>("VBoxContainer/ScrollContainer/CharacterInfo");
        
        // Get stat value labels
        statValues = new Label[4];
        statValues[0] = GetNode<Label>("VBoxContainer/StatsGrid/StrengthValue");
        statValues[1] = GetNode<Label>("VBoxContainer/StatsGrid/AgilityValue");
        statValues[2] = GetNode<Label>("VBoxContainer/StatsGrid/IntelligenceValue");
        statValues[3] = GetNode<Label>("VBoxContainer/StatsGrid/CharismaValue");
        
        skillsValue = GetNode<Label>("VBoxContainer/SkillsGrid/SkillsValue");

        // Get main scene references
        var mainScene = GetTree().Root.GetNode<Main>("Main");
        playerCharacter = mainScene.PlayerCharacter;

        // Connect back button
        GetNode<Button>("VBoxContainer/BackButton").Pressed += OnBackButtonPressed;

        // Update UI
        UpdateCharacterInfo();
        UpdateStats();
        UpdateSkills();
    }

    private void UpdateCharacterInfo()
    {
        characterInfo.Text = $"[b]Character Information[/b]\n" +
                           $"Name: {playerCharacter.Name}\n" +
                           $"Profession: {playerCharacter.Profession}\n" +
                           $"Rating: {playerCharacter.Rating}\n\n" +
                           $"[b]Background[/b]\n" +
                           $"{playerCharacter.Background}";
    }

    private void UpdateStats()
    {
        statValues[0].Text = playerCharacter.Strength.ToString();
        statValues[1].Text = playerCharacter.Agility.ToString();
        statValues[2].Text = playerCharacter.Intelligence.ToString();
        statValues[3].Text = playerCharacter.Charisma.ToString();
    }

    private void UpdateSkills()
    {
        var skills = new StringBuilder();
        foreach (var skill in playerCharacter.Skills)
        {
            skills.AppendLine($"{skill.Key}: {skill.Value}");
        }
        skillsValue.Text = skills.ToString();
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
                    case Key.B:
                        OnBackButtonPressed();
                        break;
                }
            }
        }
    }
} 