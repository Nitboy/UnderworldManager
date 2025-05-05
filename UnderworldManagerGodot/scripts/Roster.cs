using Godot;
using System;
using System.Collections.Generic;
using UnderworldManager.Core.Models;
using UnderworldManager.Business;

public partial class Roster : Control
{
    private Gang playerGang;
    private RichTextLabel gangInfo;
    private VBoxContainer memberList;
    private RichTextLabel memberDetails;
    private List<Button> memberButtons;
    private Character selectedMember;

    public override void _Ready()
    {
        // Get references to UI elements
        gangInfo = GetNode<RichTextLabel>("VBoxContainer/GangInfo");
        memberList = GetNode<VBoxContainer>("VBoxContainer/ScrollContainer/MemberList");
        memberDetails = GetNode<RichTextLabel>("VBoxContainer/MemberDetails");

        // Get main scene references
        var mainScene = GetTree().Root.GetNode<Main>("Main");
        playerGang = mainScene.PlayerGang;

        // Connect back button
        GetNode<Button>("VBoxContainer/BackButton").Pressed += OnBackButtonPressed;

        // Initialize member buttons list
        memberButtons = new List<Button>();

        // Update UI
        UpdateGangInfo();
        PopulateMemberList();
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

    private void PopulateMemberList()
    {
        // Clear existing buttons
        foreach (var button in memberButtons)
        {
            button.QueueFree();
        }
        memberButtons.Clear();

        // Create buttons for each member
        foreach (var member in playerGang.Members)
        {
            var button = new Button();
            button.Text = $"{member.Name} ({member.Profession}, Rating: {member.Rating})";
            button.Pressed += () => OnMemberSelected(member);
            memberList.AddChild(button);
            memberButtons.Add(button);
        }
    }

    private void OnMemberSelected(Character member)
    {
        selectedMember = member;
        UpdateMemberDetails();
    }

    private void UpdateMemberDetails()
    {
        if (selectedMember == null) return;

        memberDetails.Text = $"[b]Member Details[/b]\n" +
                           $"Name: {selectedMember.Name}\n" +
                           $"Profession: {selectedMember.Profession}\n" +
                           $"Rating: {selectedMember.Rating}\n\n" +
                           $"[b]Attributes[/b]\n" +
                           $"Strength: {selectedMember.Strength}\n" +
                           $"Agility: {selectedMember.Agility}\n" +
                           $"Intelligence: {selectedMember.Intelligence}\n" +
                           $"Charisma: {selectedMember.Charisma}";
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