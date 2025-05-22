using System.Collections;
using UnityEngine;

/// <summary>
/// A baseball machine that continuously spawns and launches sphere (baseball) prefabs
/// based on a selected difficulty setting, using a coroutine for timed cycles.
/// You can start Easy/Medium/Hard modes via dedicated functions (e.g. Attach to UI Buttons).
/// </summary>
public class BaseballMachine : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject baseballPrefab;

    [Header("Timing Settings")]
    public float startDelay = 2f;
    public float intervalEasy = 1f;
    public float intervalMedium = 0.5f;
    public float intervalHard = 0.2f;

    [Header("Cycle Settings")]
    public int ballsPerCycleEasy = 5;
    public int ballsPerCycleMedium = 10;
    public int ballsPerCycleHard = 20;

    [Header("Launch Settings")]
    public float launchStrength = 10f;

    // Internally tracks current difficulty
    private Difficulty currentDifficulty;
    private Coroutine machineCoroutine;

    /// <summary>
    /// Starts the machine on Easy difficulty.
    /// </summary>
    public void StartEasy()
    {
        StartWithDifficulty(Difficulty.Easy);
    }

    /// <summary>
    /// Starts the machine on Medium difficulty.
    /// </summary>
    public void StartMedium()
    {
        StartWithDifficulty(Difficulty.Medium);
    }

    /// <summary>
    /// Starts the machine on Hard difficulty.
    /// </summary>
    public void StartHard()
    {
        StartWithDifficulty(Difficulty.Hard);
    }

    /// <summary>
    /// Stops any running machine coroutine.
    /// </summary>
    public void StopMachine()
    {
        if (machineCoroutine != null)
        {
            StopCoroutine(machineCoroutine);
            machineCoroutine = null;
        }
    }

    /// <summary>
    /// Helper to set difficulty and start the coroutine.
    /// </summary>
    private void StartWithDifficulty(Difficulty difficulty)
    {
        StopMachine();
        currentDifficulty = difficulty;
        machineCoroutine = StartCoroutine(RunMachine());
    }

    /// <summary>
    /// Coroutine that waits for the initial delay, then continuously fires cycles of baseballs.
    /// </summary>
    private IEnumerator RunMachine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            int ballsToShoot;
            float interval;

            switch (currentDifficulty)
            {
                case Difficulty.Medium:
                    ballsToShoot = ballsPerCycleMedium;
                    interval = intervalMedium;
                    break;
                case Difficulty.Hard:
                    ballsToShoot = ballsPerCycleHard;
                    interval = intervalHard;
                    break;
                case Difficulty.Easy:
                default:
                    ballsToShoot = ballsPerCycleEasy;
                    interval = intervalEasy;
                    break;
            }

            for (int i = 0; i < ballsToShoot; i++)
            {
                Launch();
                yield return new WaitForSeconds(interval);
            }
        }
    }

    /// <summary>
    /// Instantiates the baseball prefab at the spawnPoint and applies an impulse forward.
    /// </summary>
    private void Launch()
    {
        if (spawnPoint == null || baseballPrefab == null)
        {
            Debug.LogWarning("BaseballMachine: Missing spawnPoint or baseballPrefab reference.");
            return;
        }

        GameObject ball = Instantiate(baseballPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>() ?? ball.AddComponent<Rigidbody>();
        rb.AddForce(spawnPoint.forward * launchStrength, ForceMode.Impulse);
    }
}
