using UnityEngine;

public class CombinationLockController : MonoBehaviour
{
    [Header("Digits")]
    public CombinationLockDigit leftDigit;
    public CombinationLockDigit middleDigit;
    public CombinationLockDigit rightDigit;

    [Header("Correct Combination")]
    [Range(0, 9)] public int correctLeft;
    [Range(0, 9)] public int correctMiddle;
    [Range(0, 9)] public int correctRight;

    [Header("Chest Lid")]
    public Transform chestLidHinge;
    public float openAngle = 45f;
    public float openSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AK.Wwise.Event chestOpenSound;

    private bool isOpen = false;
    private int[] currentCombination = new int[3];

    void Start()
    {
        leftDigit.digitIndex = 0;
        middleDigit.digitIndex = 1;
        rightDigit.digitIndex = 2;

        leftDigit.lockController = this;
        middleDigit.lockController = this;
        rightDigit.lockController = this;
    }

    public void UpdateDigit(int index, int value)
    {
        currentCombination[index] = value;
        CheckCombination();
    }

    void CheckCombination()
    {
        if (currentCombination[0] == correctLeft &&
            currentCombination[1] == correctMiddle &&
            currentCombination[2] == correctRight)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        if (isOpen) return;
        isOpen = true;

        chestOpenSound?.Post(gameObject);

        StartCoroutine(OpenLid());
    }

    System.Collections.IEnumerator OpenLid()
    {
        float elapsed = 0f;
        Quaternion startRot = chestLidHinge.localRotation;
        Quaternion endRot = Quaternion.Euler(openAngle, 0, 0) * startRot;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * openSpeed;
            chestLidHinge.localRotation = Quaternion.Slerp(startRot, endRot, elapsed);
            yield return null;
        }

        chestLidHinge.localRotation = endRot;
    }
}