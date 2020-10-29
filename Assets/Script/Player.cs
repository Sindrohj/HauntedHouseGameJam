using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObject
{

	public int pointsPerCandy = 10;
	public float restartLevelDelay = 1f;
    public Text candyText;

	private Animator animator;
	private int candy;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        candy = GameManager.instance.playerCandyPoints;

        candyText.text = "Candy: " + candy;

        base.Start();
    }

    private void OnDisable(){
    	GameManager.instance.playerCandyPoints = candy;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if(horizontal != 0)
        	vertical = 0;

        if(horizontal != 0 || vertical != 0)
        {
            AttemptMove <Enemy> (horizontal, vertical);
        }

    }

    protected override void AttemptMove <T> (int xDir, int yDir){
    	candy--;
        candyText.text = "Candy: " + candy;
        base.AttemptMove <T> (xDir, yDir);
    	RaycastHit2D hit;

    	CheckIfGameOver();

    	GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other){
    	if(other.tag == "Exit"){
    		Invoke("Restart", restartLevelDelay);
    		enabled = false;
    	} else if (other.tag == "Candy"){
    		candy += pointsPerCandy;
            candyText.text = "+" + pointsPerCandy + " Candy: " + candy;
    		other.gameObject.SetActive(false);
    	}
    }

    protected override void OnCantMove <T> (T component){

    }

    private void Restart(){
    	Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseCandy (int loss){
    	candy -= loss;
        candyText.text = "-" + loss + " Candy: " + candy;
    	CheckIfGameOver();
    }

    private void CheckIfGameOver(){
    	if(candy <= 0){
            candy = 100;
    		GameManager.instance.GameOver();
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
    }

    IEnumerator RestartGameWait()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(4);

    }


}

