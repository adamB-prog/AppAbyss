import {HttpClient} from "@angular/common/http";
import {ILoginModel} from "../../../interfaces/Dtos/login-model.interface";
import {IRegisterModel} from "../../../interfaces/Dtos/register-model.interface";
import {ApiService} from "./api.service";
import {Injectable} from "@angular/core";
import {ITokenModel} from "../../../interfaces/login/token-model.interface";
import {Observable} from "rxjs";

/**
 * Service for handling authentication-related API calls.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthApiService extends ApiService {

  protected readonly authUrl:string;
  constructor(http: HttpClient) {
      super(http);
      this.authUrl = this.apiBaseUrl + '/auth';
  }

  /**
   * Sends a login request to the server.
   * @param loginModel - The login model containing the user's credentials.
   * @returns An Observable that emits the token model upon successful login.
   */
  login(loginModel:ILoginModel):Observable<ITokenModel> {
      return this.http.post<ITokenModel>(this.authUrl + '/login', loginModel);
  }

  /**
   * Sends a registration request to the server.
   * @param registerModel - The registration model containing the user's information.
   * @returns An Observable that emits the response from the server.
   */
  register(registerModel: IRegisterModel) {
      return this.http.post(this.authUrl  + '/register', registerModel);
  }
}
