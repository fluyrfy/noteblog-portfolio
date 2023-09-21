function getCategoryValue(categoryId) {
    try {
        const request = $.ajax({
            url: `/api/categories/get/${categoryId}`,
            method: 'GET',
            dataType: 'json',
        }).done(function (category) {
            $("#editCategoryName").val(category.name);
            $("#editCategoryDescription").val(category.description);
        }).fail(function (xhr, status, error) {
            console.error(status, error);
        });
    } catch (error) {
        console.error('error:', error);
    }
}

function getUserValue(userId) {
    try {
        const request = $.ajax({
            url: `/api/users/get/${userId}`,
            method: 'GET',
            dataType: 'json',
        }).done(function (user) {
            $("#editUserName").val(user.name);
            $("#editUserRole").val(user.role);
        }).fail(function (xhr, status, error) {
            console.error(status, error);
        });
    } catch (error) {
        console.error('error:', error);
    }
}