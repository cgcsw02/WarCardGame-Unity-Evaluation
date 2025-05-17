# WarCardGame-Unity-Evaluation
Unity-Evaluation for Tamatem Company

# Unity Card Game Project

Below is a reformatted and polished version of the README for your "WarCardGame-Unity-Evaluation" project, designed to be copied directly into your `README.md` file on GitHub. This version maintains all the content from your provided README, enhances its visual appeal with consistent markdown formatting, and ensures it’s clear, professional, and evaluator-friendly for the Tamatem Company technical assessment. It accurately reflects your **manual dependency injection** implementation, highlights advanced Unity techniques (UniTask, DOTween, etc.), and proposes future improvements (Zenject, Addressables, ScriptableObjects) to maximize bonus points. The structure is streamlined for readability, with proper spacing, bullet points, and code blocks.

---

```markdown
# WarCardGame-Unity-Evaluation

**Unity Technical Assessment for Tamatem Company**

## Introduction

This Unity project is a single-player implementation of the "War" card game, developed for a technical assessment for Tamatem Company. The player competes against a bot, drawing cards from the Deck of Cards API over up to 8 rounds. The first to reach 5 points or the player with the highest score after 8 rounds is declared the winner. The project showcases a clean, modular architecture with advanced Unity techniques, including asynchronous operations, manual dependency injection, animations, sound effects, and robust error handling.

## Game Rules

- The game runs for up to 8 rounds.
- Each round:
  - The player clicks "Draw" to draw a random card.
  - After a random delay (0.5–1s), the bot draws a random card.
  - Cards are compared by value (Ace > King > Queen > ... > 2).
  - The higher card awards 1 point to the winner; ties award no points.
- The game ends after 8 rounds or when a player reaches 5 points.
- The player with the most points wins; if tied, the game is a draw.

## Code Structure and Logic

### Architecture

- **Folder Structure**: Organized for clarity and maintainability:
  - `Scripts/Presentation`: Manages UI, game flow, and initialization (`GameBootstrapper`, `GameManager`, `UIManager`, `SimpleSoundPlayer`).
  - `Scripts/Model`: Defines data structures (`Card`, `CardComparer`).
  - `Scripts/Logic`: Handles game rules and API interactions (`GameLogic`, `DeckApiService`, interfaces).
- **Manual Dependency Injection**: `GameBootstrapper` injects dependencies (e.g., `DeckApiService`, `GameLogic`, `UIManager`) into `GameManager` via an `Initialize` method, ensuring loose coupling and modularity.
- **Asynchronous Operations**: UniTask enables non-blocking API calls, bot delays, and animations, maintaining responsiveness.
- **Interfaces**: `IGameLogic` and `IDeckApiService` promote testability and decoupling.

### Game Logic

- **GameManager**: Orchestrates game flow, managing state transitions (main menu, gameplay, end screen), button events, and round execution.
- **GameLogic**: Implements core rules (card comparison, scoring, game end conditions), separated from presentation.
- **DeckApiService**: Handles async API calls to the Deck of Cards API, deserializing JSON responses (via Newtonsoft.Json) into `Card` models.
- **Scene**: A single `MainScene` hosts all components, initialized by `GameBootstrapper` to ensure proper setup and avoid null references.

## API Integration

- Connects to the Deck of Cards API (https://deckofcardsapi.com/) for deck creation, card draws, and deck status checks.
- Uses UniTask for asynchronous requests and Newtonsoft.Json for deserialization.
- Handles network errors by logging issues and displaying UI messages, preventing crashes.

## Optional or Extra Features

- **UniTask**: Provides clean async/await syntax for API calls, bot delays, and animations, enhancing performance and readability.
- **Manual Dependency Injection**: Injects dependencies via `GameBootstrapper`, promoting modularity and avoiding tight coupling.
- **Animations**: DOTween delivers smooth card scaling, UI fades, and a "Bot is thinking" text animation.
- **Sound Effects**: Unity’s audio system plays card flips, win/lose sounds, and background music via `SimpleSoundPlayer`.
- **Error Handling**: Gracefully manages API failures with fallback UI messages.
- **Debug Logging**: Extensive logging aids development and debugging.

## Instructions for Running and Testing

### Requirements

- **Unity Version**: 6000.0 (URP)
- **Dependencies**:
  - UniTask (via Package Manager)
  - DOTween (via Package Manager)
  - Newtonsoft.Json (via Package Manager or Plugins folder)
  - TextMeshPro (included with Unity)

### Setup Instructions

1. Clone the repository:
   ```bash
   git clone <your-github-repo-url>
   ```
2. Open the project in Unity 6000.0.
3. Install dependencies via the Unity Package Manager (or ensure Newtonsoft.Json is in Plugins).
4. Open `MainScene` from the `Scenes` folder.
5. Press "Play" to launch.

### Testing the Game

- **Main Menu**: Click "Start" to begin.
- **Gameplay**: Click "Draw" to initiate rounds, observing card animations, bot "thinking" delay, sounds, and score updates.
- **End Screen**: Appears after 8 rounds or when a player reaches 5 points, showing the result and a "Restart" button.
- Test API error handling by disabling internet to verify error messages.
- Confirm animations (card scaling, UI fades), sounds (card flips, win/lose), and UI updates (scores, messages).

## Design Decisions

- **Manual Dependency Injection**: Chosen for simplicity, leveraging Unity’s Inspector and an `Initialize` method to balance modularity with minimal setup.
- **UniTask**: Preferred over coroutines for modern async/await syntax and performance.
- **DOTween**: Selected for robust, easy-to-use animations.
- **Single Scene**: Simplifies state management and ensures consistent initialization.
- **Inspector Assignments**: Uses `[SerializeField]` for dependency setup, with validation to prevent misconfigurations.

## Reflections on Improvements

With additional time, I would enhance the project by:

- **Dependency Injection Framework**: Adopt **Zenject** for a robust, scalable DI solution, automating dependency management and improving testability.
- **Addressables**: Use Unity’s Addressables for efficient card image loading, reducing memory usage and enabling dynamic asset management.
- **ScriptableObjects**: Implement ScriptableObjects for game settings (e.g., card ranks, animation timings) to decouple configuration.
- **State Machine**: Add a formal state machine for clearer game state management (menu, playing, end).
- **Object Pooling**: Pool card images to reduce instantiation overhead.
- **War Mechanic**: Include the traditional "War" tiebreaker (drawing additional cards) for authenticity.
- **Unit Tests**: Develop tests for `GameLogic` and `DeckApiService` to ensure reliability.

These enhancements would improve scalability, performance, and gameplay depth, building on the project’s strong foundation.
