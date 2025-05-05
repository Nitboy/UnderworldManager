using Godot;
using System;
using UnderworldManager.Core.Models;
using UnderworldManager.Business;

public partial class Main : Node
{
    private Character playerCharacter;
    private Gang playerGang;
    private DiceRoller diceRoller;
    private CharGen charGen;
    private GangGen gangGen;

    public Character PlayerCharacter => playerCharacter;
    public Gang PlayerGang => playerGang;

    public override void _Ready()
    {
        // Initialize game systems
        diceRoller = new DiceRoller();
        charGen = new CharGen();
        gangGen = new GangGen();

        // Generate player character and gang
        playerCharacter = charGen.CreateCharacter(1, null, true);
        playerGang = gangGen.CreateGang(1, playerCharacter);

        // Display initial game state
        DisplayGameState();

        // Connect button signals
        GetNode<Button>("UI/Actions/StreetsButton").Pressed += OnStreetsButtonPressed;
        GetNode<Button>("UI/Actions/TrainingButton").Pressed += OnTrainingButtonPressed;
        GetNode<Button>("UI/Actions/RosterButton").Pressed += OnRosterButtonPressed;
        GetNode<Button>("UI/Actions/PlayerButton").Pressed += OnPlayerButtonPressed;
        GetNode<Button>("UI/Actions/QuitButton").Pressed += OnQuitButtonPressed;

        // Enable input processing
        ProcessMode = ProcessModeEnum.Always;
    }

    private void DisplayGameState()
    {
        var gameLog = GetNode<RichTextLabel>("UI/GameLog");
        gameLog.Text = $"[center][b]Welcome to Underworld Manager[/b][/center]\n\n" +
                      $"In the dark alleys of the city, you lead a gang of thieves, cutthroats, and scoundrels.\n" +
                      $"Your decisions will shape the fate of your crew and determine whether you rise to power\n" +
                      $"or fall into obscurity.\n\n" +
                      $"[b]Controls:[/b]\n" +
                      $"• S - Hit the Streets\n" +
                      $"• T - Training\n" +
                      $"• R - View Roster\n" +
                      $"• P - View Player Details\n" +
                      $"• Q - Quit\n\n" +
                      $"[b]Your Character:[/b]\n" +
                      $"{playerCharacter}\n\n" +
                      $"[b]Gang Information:[/b]\n" +
                      $"{playerGang}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed && !eventKey.Echo)
        {
            GD.Print($"Key pressed: {eventKey.Keycode}");
            HandleKeyInput(eventKey.Keycode);
        }
    }

    private void HandleKeyInput(Key keycode)
    {
        switch (keycode)
        {
            case Key.S: // Hit the streets
                GD.Print("S key pressed - Going to streets");
                OnStreetsButtonPressed();
                break;
            case Key.T: // Training
                GD.Print("T key pressed - Going to training");
                OnTrainingButtonPressed();
                break;
            case Key.R: // View roster
                GD.Print("R key pressed - Going to roster");
                OnRosterButtonPressed();
                break;
            case Key.P: // View player details
                GD.Print("P key pressed - Going to player details");
                OnPlayerButtonPressed();
                break;
            case Key.Q: // Quit
                GD.Print("Q key pressed - Quitting");
                OnQuitButtonPressed();
                break;
        }
    }

    private void OnStreetsButtonPressed()
    {
        GD.Print("Streets button pressed");
        GetTree().ChangeSceneToFile("res://scenes/street.tscn");
    }

    private void OnTrainingButtonPressed()
    {
        GD.Print("Training button pressed");
        GetTree().ChangeSceneToFile("res://scenes/training.tscn");
    }

    private void OnRosterButtonPressed()
    {
        GD.Print("Roster button pressed");
        GetTree().ChangeSceneToFile("res://scenes/roster.tscn");
    }

    private void OnPlayerButtonPressed()
    {
        GD.Print("Player button pressed");
        GetTree().ChangeSceneToFile("res://scenes/character.tscn");
    }

    private void OnQuitButtonPressed()
    {
        GD.Print("Quit button pressed");
        GetTree().Quit();
    }
} 