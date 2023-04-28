



using System.Collections;

public interface ILevelManager {

    public IEnumerator SpawnBallsCoroutine();

    public IEnumerator StartGetReadyBannerCoroutine();

    public void StopGetReadyBanner();

    public IEnumerator StartCountdownBannerCoroutine();

    public void StopCountdownBanner();

    public IEnumerator StartEndingBanner();

    public void StopEndingBanner();

    public void MoveBallsToMiddle();

    public bool IsWinConditionMet();

    public bool IsWon(); 
    







}