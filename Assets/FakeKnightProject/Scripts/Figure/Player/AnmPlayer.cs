using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnmPlayer : MonoBehaviour
{
    [SerializeField] private Animator anm;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private UseSkill useSkill;
    void UpdateAnimation()
    {
        if (!playerMove.isSkill)
        {
            if (playerMove.horizontalInput > 0.01f)
            {
                if (playerMove.sprite.flipX)
                    playerMove.sprite.flipX = !playerMove.sprite.flipX;
                anm.Play("runLR");
                playerMove.isLR = true;
                playerMove.isUD = false;
                playerMove.isU = false;
                playerMove.isD = false;
            }
            else if (playerMove.horizontalInput < -0.01f)
            {
                if (!playerMove.sprite.flipX)
                    playerMove.sprite.flipX = !playerMove.sprite.flipX;
                anm.Play("runLR");
                playerMove.isLR = true;
                playerMove.isUD = false;
                playerMove.isU = false;
                playerMove.isD = false;
            }
            else if (playerMove.horizontalInput == 0f)
            {
                playerMove.isLR = false;
            }


            if (playerMove.verticalInput > 0.01f)
            {
                anm.Play("runBehind");
                playerMove.isUD = true;
                playerMove.isU = true;
                playerMove.isD = false;
                playerMove.isLR = false;
            }
            else if (playerMove.verticalInput < -0.01f)
            {
                anm.Play("runFront");
                playerMove.isUD = true;
                playerMove.isD = true;
                playerMove.isU = false;
                playerMove.isLR = false;
            }
            else if (playerMove.verticalInput == 0f)
            {
                playerMove.isUD = false;
            }

            if (!playerMove.isLR && !playerMove.isUD)
            {
                if (playerMove.isU)
                    anm.Play("idleBehind");
                else if (playerMove.isD)
                    anm.Play("idleFront");
                else
                    anm.Play("idleLR");
            }
        }
        else if (playerMove.isChooseSk1)
        {
            playerMove.isChooseSk1 = false;
            if (playerMove.isU)
            {
                anm.Play("skillBehind");
            }
            else if (playerMove.isD)
            {
                anm.Play("skillFront");
            }
            else
            {
                anm.Play("skillLR");
            }
            Invoke("resetSkill", 0.3f);
        }
    }
    public void UpdateAnimationMobile()
    {
        if (!playerMove.isSkill)
        {
            if (playerMove.velocity_.x > 0.01f && Mathf.Abs(playerMove.velocity_.x) >= 0.4f)
            {
                if (playerMove.sprite.flipX)
                    playerMove.sprite.flipX = !playerMove.sprite.flipX;
                anm.Play("runLR");
                playerMove.isLR = true;
                playerMove.isUD = false;
                playerMove.isU = false;
                playerMove.isD = false;
            }
            else if (playerMove.velocity_.x < -0.01f && Mathf.Abs(playerMove.velocity_.x) >= 0.4f)
            {
                if (!playerMove.sprite.flipX)
                    playerMove.sprite.flipX = !playerMove.sprite.flipX;
                anm.Play("runLR");
                playerMove.isLR = true;
                playerMove.isUD = false;
                playerMove.isU = false;
                playerMove.isD = false;
            }
            else if (playerMove.velocity_.x == 0f)
            {
                playerMove.isLR = false;
            }


            if (playerMove.velocity_.y > 0.01f && Mathf.Abs(playerMove.velocity_.x) < 0.4f)
            {
                anm.Play("runBehind");
                playerMove.isUD = true;
                playerMove.isU = true;
                playerMove.isD = false;
                playerMove.isLR = false;
            }
            else if (playerMove.velocity_.y < -0.01f && Mathf.Abs(playerMove.velocity_.x) < 0.4f)
            {
                anm.Play("runFront");
                playerMove.isUD = true;
                playerMove.isD = true;
                playerMove.isU = false;
                playerMove.isLR = false;
            }
            else if (playerMove.velocity_.y == 0f)
            {
                playerMove.isUD = false;

            }

            if (!playerMove.isLR && !playerMove.isUD)
            {
                if (playerMove.isU)
                    anm.Play("idleBehind");
                else if (playerMove.isD)
                    anm.Play("idleFront");
                else
                    anm.Play("idleLR");
            }
        }
        else if (playerMove.isChooseSk1)
        {
            playerMove.isChooseSk1 = false;
            if (playerMove.isU)
            {
                anm.Play("skillBehind");
            }
            else if (playerMove.isD)
            {
                anm.Play("skillFront");
            }
            else
            {
                anm.Play("skillLR");
            }
            Invoke("ResetSkill", 0.3f);
        }
    }
    void ResetSkill()
    {
        // Call resetSkill method from useSkill script
        useSkill.resetSkill();
    }
}
