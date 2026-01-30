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
        string rawInput = inputField.text;
        string command = rawInput.ToLower().Trim();

        AnimationIntent intent = InferIntent(command);
        float speed = InferSpeed(command);

        ApplyIntent(intent, speed);
    }

    AnimationIntent InferIntent(string command)
    {
        if (command.Contains("wave") || command.Contains("hello") || command.Contains("hi"))
            return AnimationIntent.Wave;

        if (command.Contains("jump") || command.Contains("hop"))
            return AnimationIntent.Jump;

        if (command.Contains("run") || command.Contains("sprint"))
            return AnimationIntent.Run;

        if (command.Contains("walk") || command.Contains("move"))
            return AnimationIntent.Walk;

        if (command.Contains("stop") || command.Contains("idle") || command.Contains("rest"))
            return AnimationIntent.Idle;

        return AnimationIntent.Unknown;
    }

    // ⭐ NEW: speed inference
    float InferSpeed(string command)
    {
        if (command.Contains("very fast"))
            return 1.8f;

        if (command.Contains("fast"))
            return 1.4f;

        if (command.Contains("slow") || command.Contains("slowly"))
            return 0.6f;

        if (command.Contains("very slow"))
            return 0.4f;

        return 1.0f; // default speed
    }

    void ApplyIntent(AnimationIntent intent, float speed)
    {
        // Reset continuous states
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        // Reset speed
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
                animator.SetTrigger("Wave");
                break;

            case AnimationIntent.Jump:
                animator.SetTrigger("Jump");
                break;

            case AnimationIntent.Idle:
                // default idle
                break;

            case AnimationIntent.Unknown:
                Debug.Log("Unknown command, staying idle.");
                break;
        }
    }
}