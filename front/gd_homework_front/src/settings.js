let API_SERVER_VAL = "";
switch (process.env.NODE_ENV) {
    case 'development':
        API_SERVER_VAL = 'https://localhost:44325/api/' ;
        break;
    case 'production':
        API_SERVER_VAL = 'http://localhost:8080/api/';
        break;
    default:
        API_SERVER_VAL = 'https://localhost:44325/api/';
        break;
}

export const API_SERVER = API_SERVER_VAL;