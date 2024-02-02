
var PublicCommon = {
    showCompanyRelatedFields: function (companyTyepeId) {
        if (companyTyepeId.value == 2) {
            $("#userTelefonos").css("display", "none");
            $("#userAddress").css("display", "none");
            $("#companyName").css("display", "block");
            $("#companyDepartment").css("display", "block");
            $("#companyTelefon").css("display", "block");
            $("#companyAddress").css("display", "block");
        }
        else {
            $("#userTelefonos").css("display", "block");
            $("#userAddress").css("display", "block");
            $("#companyName").css("display", "none");
            $("#companyDepartment").css("display", "none");
            $("#companyTelefon").css("display", "none");
            $("#companyAddress").css("display", "none");
        }
    }
};