import { access } from "fs";
import { makeAutoObservable, runInAction } from "mobx";
import { date } from "yup/lib/locale";
import { history } from "../..";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";

export default class UserStore{
    user: User | null = null;
    fbAccessToken : string | null = null;
    fbLoading : boolean = false;
    freshTokenTimeout : any;

    constructor() {
        makeAutoObservable(this)
    }

    get isLoggedIn() {
        return !! this.user;
    }

    login = async ( creds: UserFormValues) => {
        try{
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
            runInAction(()=> this.user = user);

            history.push('/activities');
            store.modalStore.closeModal()
        } catch(error) {
            throw error;
        }   
    }

    getFacebookLoginStatus = async () =>{
        window.FB.getLoginStatus(response => {
            if(response.status === 'connected'){
                this.fbAccessToken = response.authResponse.accessToken;
            }
        })
    }

    facebookLogin = async () => {
        this.fbLoading = true;
        const apiLogin = (accessToken:string) => {
            agent.Account.fbLogin(accessToken).then( user=> {
                store.commonStore.setToken(user.token);
                this.startRefreshTokenTimer(user);
                runInAction(()=>{
                    this.user = user;
                    this.fbLoading = false;
                })
                history.push('/activities');
            }).catch( error => {
                console.log(error);
                runInAction(()=>this.fbLoading = false);
            })
        }
        if(this.fbAccessToken){
            apiLogin(this.fbAccessToken);
        } else {
            window.FB.login( response =>{
                apiLogin(response.authResponse.accessToken)
            }, {scope:'public_profile,email'})
        }
    }

    refreshToken = async () => {
        try {
            this.stopRefreshTokenTimer();
            const user = await agent.Account.refreshToken();
            runInAction(()=> this.user = user);
            store.commonStore.setToken(user.token);                        
            this.stopRefreshTokenTimer();
        } catch (error) {
            console.log(error)
        }
    }

    private startRefreshTokenTimer( user: User){
        const jwtToken = JSON.parse(atob(user.token.split('.')[1]));
        const expires = new Date(jwtToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (60*1000);        

        this.freshTokenTimeout = setTimeout( this.refreshToken, timeout);
    }

    private stopRefreshTokenTimer() {
        clearTimeout( this.freshTokenTimeout);
    }

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt');
        this.user = null;
        history.push('/');
    }

    getUser = async () => {
        try{
            const user = await agent.Account.current();
            store.commonStore.setToken(user.token);
            runInAction( ()=> this.user = user );
            this.startRefreshTokenTimer(user);
        } catch(error){
            console.log(error);
        }
    }

    register = async (creds: UserFormValues) => {

        try{
            await agent.Account.register(creds);
            history.push(`/account/success?email=${creds.email}`);
            store.modalStore.closeModal()        
        } catch(error) {
            throw error;
        }   
    }

    setImage = (image: string)=>{
        if( this.user){
            this.user.image = image
        }    
    }

    setDisplayName = async (displayName: string) => {
        if(this.user)        {
            this.user.displayName = displayName
        }
    }
}