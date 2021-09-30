import { Formik, Form } from "formik";
import React from "react";
import { Button } from "semantic-ui-react";
import * as Yup from 'yup';
import { useStore } from "../../app/stores/store";
import MyTextInput from "../../app/common/form/MyTextInput";
import MyTextArea from "../../app/common/form/MyTextArea";

interface Props {
    setEditMode: (editMode: boolean)=> void;
}

export default function ProfileEditForm({setEditMode}:Props) {

    const {profileStore: {profile,updateProfile}} = useStore();

    return (
        <Formik 
            initialValues={{displayName: profile?.displayName, bio:profile?.bio}} 
            onSubmit={values => {
                updateProfile(values).then(()=>{
                    setEditMode(false);
                })
            }}
            validationSchema={ Yup.object({
                displayName: Yup.string().required()
            })}
            >
            {({isValid, isSubmitting, dirty})=>(
            <Form autoComplete='off' className='ui form'>
                <MyTextInput name='displayName' placeholder='Display Name'/>                        
                <MyTextArea rows={3}  placeholder='Bio' name='bio'/>

                <Button
                    positive 
                    type='submit'
                    loading={isSubmitting}
                    content='Update profile'
                    floated='right'
                    disabled={!dirty || !isValid}
                    />
            </Form>
        )}       
        </Formik>
    )
}