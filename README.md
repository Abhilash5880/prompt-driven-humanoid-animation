# 🧠 Prompt-Driven Humanoid Animation using an AI Agent

## 📌 Project Overview

This project demonstrates a **prompt-driven AI agent** that interprets **natural language commands** and controls **humanoid character animation** in real time using Unity.

Instead of attempting unstable, fully automated animation generation, the system focuses on **intelligent decision-making and control**, which reflects how professional animation pipelines actually work.

The AI agent:

* Reads free-form text prompts
* Infers **intent** (walk, run, wave, jump, idle)
* Infers **modifiers** (speed: slow, fast, very fast)
* Drives a structured **Animator Controller**
* Works across **multiple humanoid characters** simultaneously

---

## Demo - [click here](https://youtu.be/v6mSM30lvm0)
---
## 🎯 Motivation

The original ambition of the project was to explore:

> *“Can a natural language prompt be used to animate a 3D character, similar to how text-to-image systems work?”*

Early experimentation revealed that:

* Fully automated humanoid retargeting via CLI pipelines is fragile
* Animation creation ≠ animation control
* Industry tools rely on **human-verified rigs** and **predefined motion assets**

This led to a **critical pivot**:

> **AI should reason about *what* to do, not directly manipulate bones or generate motion data.**

This project reflects that insight.

---

## 🧱 Final System Architecture

```
User Prompt (Text)
        ↓
AI Agent (Rule-based Reasoning)
        ↓
Animator Parameters (Bool / Trigger / Speed)
        ↓
Unity Animator Controller
        ↓
Humanoid Character Animation
```

---

## 🛠️ Tools & Technologies

* **Unity 2022.3 LTS**
* **Unity Humanoid Animation System**
* **Mixamo** (pre-rigged characters & animations)
* **C#**
* **TextMeshPro (TMP)**
* **Animator State Machines**

No external ML APIs are required.

---

## 🧍 Characters & Assets

* Characters:

  * **Remy**
  * **XBot**
  * More can added easily and can also be animated
* Both are Mixamo humanoids
* Same Animator Controller & Agent script works on both

### Animations Used (skeletal only, no skin)

* Idle
* Walk
* Run
* Wave
* Jump
* More can be easily added

---

## 🎭 Animator Design

### States

* `Idle` (default)
* `Walk`
* `Run`
* `Wave`
* `Jump`

### Parameters

| Parameter   | Type    | Purpose               |
| ----------- | ------- | --------------------- |
| `isWalking` | Bool    | Continuous locomotion |
| `isRunning` | Bool    | Continuous locomotion |
| `wave`      | Trigger | One-shot gesture      |
| `jump`      | Trigger | One-shot action       |

### Key Design Rules

* Locomotion uses **bools**
* Gestures use **triggers**
* `Idle` acts as the global fallback
* No animation is played directly via code

---

## 🤖 AI Agent Logic

The agent operates using a **Sense → Decide → Act** loop.

### Intent Inference

The agent extracts intent from free-form text:

Examples:

* `"walk"` → Walk
* `"run now"` → Run
* `"wave hello"` → Wave
* `"jump"` → Jump
* `"stop"` → Idle

### Modifier Inference

Speed modifiers are inferred using keywords:

| Phrase        | Speed |
| ------------- | ----- |
| slow / slowly | 0.6   |
| fast          | 1.4   |
| very fast     | 1.8   |
| default       | 1.0   |

Speed is applied via `Animator.speed`.

---

## 📄 Core Script (`AnimationAgent.cs`)

```csharp
using UnityEngine;
using TMPro;

public class AnimationAgent : MonoBehaviour
{
    public Animator animator;
    public TMP_InputField inputField;

    enum AnimationIntent
    {
        Idle,
        Walk,
        Run,
        Wave,
        Jump,
        Unknown
    }

    public void OnSendCommand()
    {
        string command = inputField.text.ToLower().Trim();

        AnimationIntent intent = InferIntent(command);
        float speed = InferSpeed(command);

        ApplyIntent(intent, speed);
    }

    AnimationIntent InferIntent(string command)
    {
        if (command.Contains("wave") || command.Contains("hello"))
            return AnimationIntent.Wave;

        if (command.Contains("jump"))
            return AnimationIntent.Jump;

        if (command.Contains("run"))
            return AnimationIntent.Run;

        if (command.Contains("walk") || command.Contains("move"))
            return AnimationIntent.Walk;

        if (command.Contains("stop") || command.Contains("idle"))
            return AnimationIntent.Idle;

        return AnimationIntent.Unknown;
    }

    float InferSpeed(string command)
    {
        if (command.Contains("very fast"))
            return 1.8f;
        if (command.Contains("fast"))
            return 1.4f;
        if (command.Contains("slow"))
            return 0.6f;

        return 1.0f;
    }

    void ApplyIntent(AnimationIntent intent, float speed)
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.speed = 1.0f;

        switch (intent)
        {
            case AnimationIntent.Walk:
                animator.speed = speed;
                animator.SetBool("isWalking", true);
                break;

            case AnimationIntent.Run:
                animator.speed = speed;
                animator.SetBool("isRunning", true);
                break;

            case AnimationIntent.Wave:
                animator.SetTrigger("wave");
                break;

            case AnimationIntent.Jump:
                animator.SetTrigger("jump");
                break;
        }
    }
}
```

---

## 🧪 Example Prompts

Try typing:

* `walk`
* `walk slowly`
* `run fast`
* `run very fast`
* `wave hello`
* `jump`
* `stop`

---

## 🔄 Key Challenges & Pivots

### ❌ What Didn’t Work

* Blender CLI automation
* Headless animation retargeting
* Programmatic FBX rebinding
* Treating animation like image generation

### ✅ What Worked

* Human-verified humanoid rigs
* Predefined animation libraries
* AI controlling **decisions**, not bones
* Animator-driven execution

---

## 🧠 Why This Is Still an AI Agent

This project includes:

* Perception (natural language input)
* Reasoning (intent & modifier inference)
* Planning (state selection)
* Action (Animator control)

It is **rule-based AI**, which is:

* Explainable
* Deterministic
* Robust
* Industry-acceptable

---

## 🚀 Future Improvements (Optional)

* Replace rule engine with LLM
* JSON-based agent interface
* Layered animations (upper body gestures)
* Network or API-based input

---

## 🏁 Final Summary

This project demonstrates a **realistic, scalable, and defensible approach** to prompt-driven 3D animation using an AI agent. It prioritizes **engineering correctness and system design** over unstable full automation, making it suitable for real-world applications and evaluation.

---
