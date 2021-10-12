import React from "react";
import { toast } from "react-toastify";
import { Button, Header, Icon, Segment } from "semantic-ui-react";
import agent from "../../app/api/agent";
import userQuery from "../../app/common/util/hooks";


export default function RegisterSuccess() {
    const email = userQuery().get('email') as string;

    function handleConfirmEmailResend() {
        agent.Account.resendEmailConfirm(email).then( () => {
            toast.success('Verification eamil resent - please check you email');            
        }).catch(error=>console.log(error));
    }

    return(
        <Segment placeholder textAlign='center'>
            <Header icon color='green'>
                <Icon name='check'/>
                Successfully registered!
            </Header>
            <p>Please check you email (including junk email) for the Verification email</p>
            {email && 
                <>
                    <p>Didn't receive the email? Click the below button to resend</p>
                    <Button primary onCLick={handleConfirmEmailResend} content='Resend email' size='huge'/>
                </>
            }
        </Segment>

    )

}