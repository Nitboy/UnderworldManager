using Godot;
using System;
using UnderworldManager.Core.Models;
using UnderworldManager.Business;

public partial class Street : Control
{
    private Character playerCharacter;
    private Gang playerGang;
    private RichTextLabel gangInfo;
    private RichTextLabel actionLog;
    private MissionRunner missionRunner;
    private DailyJobRunner jobRunner;

    public override void _Ready()
    {
        // Get references to UI elements
        gangInfo = GetNode<RichTextLabel>("VBoxContainer/GangInfo");
        actionLog = GetNode<RichTextLabel>("VBoxContainer/ActionLog");

        // Get main scene references
        var mainScene = GetTree().Root.GetNode<Main>("Main");
        playerCharacter = mainScene.PlayerCharacter;
        playerGang = mainScene.PlayerGang;

        // Initialize runners
        missionRunner = new MissionRunner();
        jobRunner = new DailyJobRunner();

        // Connect button signals
        GetNode<Button>("VBoxContainer/ActionOptions/MissionButton").Pressed += OnMissionButtonPressed;
        GetNode<Button>("VBoxContainer/ActionOptions/JobButton").Pressed += OnJobButtonPressed;
        GetNode<Button>("VBoxContainer/ActionOptions/LayLowButton").Pressed += OnLayLowButtonPressed;
        GetNode<Button>("VBoxContainer/BackButton").Pressed += OnBackButtonPressed;

        // Update UI
        UpdateGangInfo();
    }

    private void UpdateGangInfo()
    {
        gangInfo.Text = $"[b]Gang Information[/b]\n" +
                       $"Name: {playerGang.Name}\n" +
                       $"Tier: {playerGang.Tier}\n" +
                       $"Gold: {playerGang.Gold}\n" +
                       $"Members: {playerGang.Members.Count}\n" +
                       $"Threat Level: {playerGang.ThreatLevel}";
    }

    private void OnMissionButtonPressed()
    {
        var result = missionRunner.RunMission(playerCharacter, playerGang);
        actionLog.Text += $"\n[b]Mission Results:[/b]\n" +
                         $"Success: {result.Success}\n" +
                         $"Gold Earned: {result.GoldEarned}\n" +
                         $"Threat Change: {result.ThreatChange}\n" +
                         $"Casualties: {result.Casualties}";
        UpdateGangInfo();
    }

    private void OnJobButtonPressed()
    {
        var result = jobRunner.RunJob(playerCharacter, playerGang);
        actionLog.Text += $"\n[b]Job Results:[/b]\n" +
                         $"Success: {result.Success}\n" +
                         $"Gold Earned: {result.GoldEarned}\n" +
                         $"Threat Change: {result.ThreatChange}\n" +
                         $"Casualties: {result.Casualties}";
        UpdateGangInfo();
    }

    private void OnLayLowButtonPressed()
    {
        playerGang.ThreatLevel = Math.Max(0, playerGang.ThreatLevel - 1);
        actionLog.Text += "\n[b]Laying Low:[/b]\n" +
                         "Threat level decreased by 1";
        UpdateGangInfo();
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
                    case Key.M:
                        GetNode<Button>("VBoxContainer/ActionOptions/MissionButton").EmitSignal("pressed");
                        break;
                    case Key.J:
                        GetNode<Button>("VBoxContainer/ActionOptions/JobButton").EmitSignal("pressed");
                        break;
                    case Key.L:
                        GetNode<Button>("VBoxContainer/ActionOptions/LayLowButton").EmitSignal("pressed");
                        break;
                    case Key.B:
                        OnBackButtonPressed();
                        break;
                }
            }
        }
    }
} 