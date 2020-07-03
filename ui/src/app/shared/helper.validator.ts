export  class ValidatorsHelper {

    public static isEmptyOrNull(value: string): boolean {

        return value === null || value.toString().trim().length === 0;
    }

    public static isValidEmail(email: string): boolean {

        if (ValidatorsHelper.isEmptyOrNull(email)){
            return false;
        }

        const emailreg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        return emailreg.test(email);
    }

    public static isValidPhone(phone: string) : boolean {

        if (ValidatorsHelper.isEmptyOrNull(phone)){
            return false;
        }

        const phoneReg = /\(?\d{3}\)?-? *\d{3}-? *-?\d{4}$/;

        return phoneReg.test(phone);
    }
}