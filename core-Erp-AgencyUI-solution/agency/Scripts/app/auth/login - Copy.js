/*
Creator: damien@acsghana.com
*/

var Auth = function() {
	
	var authApiUrl = agencyAPI_URL_Root + "/Auth";
	
	var auth = {};
	
    var handleLogin = function() {

        $('.login-form').validate({
            errorElement: 'span',
            errorClass: 'help-block',
            focusInvalid: false,

            invalidHandler: function(event, validator) { //display error alert on form submit   
                $('.alert-danger', $('.login-form')).show();
            },

            highlight: function(element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function(label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function(error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function(form) {
                retrieveLoginValues();
				sendLoginToServer();
            }
        });
		
		
		$("#username").rules("add", {required:true});
		$("#password").rules("add", {required:true});

        $('.login-form input').keypress(function(e) {
            if (e.which == 13) {
                if ($('.login-form').validate().form()) {
                    $('.login-form').submit(); //form validation success, call ajax form submit
                }
                return false;
            }
        });
    }
	
	function retrieveLoginValues() {
		auth.username = $('#username').val();
		auth.password = $('#password').val();
	}

	function sendLoginToServer() {

		$.ajax({
			url: authApiUrl + "/Login",
			type: 'POST',
			contentType: "application/json",
			data: JSON.stringify(auth)
		}).done(function (data) {
			if(data == null)
			{
				alert('Invalid Credentials. Check your username or password.');
			}
			else {
				alert('Login Details Verified successfuly.');
				window.location = '/Agent/Dashboard';
			}
		}).error(function (xhr, data, error) {
			//warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
			alert(xhr.responseJSON.ExceptionMessage);
			
		});
	}

    var handleForgotPassword = function() {
        $('.forgot-form').validate({
            errorElement: 'span', 
            errorClass: 'help-block',
            focusInvalid: false,

            invalidHandler: function(event, validator) { //display error alert on form submit   
				$('.alert-danger', $('.forgot-form')).show();
            },

            highlight: function(element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').addClass('has-error'); // set error class to the control group
            },

            success: function(label) {
                label.closest('.form-group').removeClass('has-error');
                label.remove();
            },

            errorPlacement: function(error, element) {
                error.insertAfter(element.closest('.input-icon'));
            },

            submitHandler: function(form) {
                retrievePasswordResetValues();
				sendPasswordResetToServer();
            }
        });
		
		$("#email").rules("add", {required:true, email: true});

        $('.forget-form input').keypress(function(e) {
            if (e.which == 13) {
                if ($('.forgot-form').validate().form()) {
                    $('.forgot-form').submit();
                }
                return false;
            }
        });
		
		function retrievePasswordResetValues() {
			auth.email = $('#email').val();
		}

		function sendPasswordResetRequestToServer() {

			$.ajax({
				url: authApiUrl + "/PasswordResetRequest",
				type: 'POST',
				contentType: "application/json",
				data: JSON.stringify(auth)
			}).done(function (data) {
				if(data == null)
				{
					alert('Account not Found!. Check your email address.');
				}
				else {
					alert('We have sent you an email.');
					window.location = '/Auth/ChangePassword';
				}
			}).error(function (xhr, data, error) {
				//warningDialog(xhr.responseJSON.ExceptionMessage, 'ERROR');
				alert(xhr.responseJSON.ExceptionMessage);
				
			});
		}

        jQuery('.forgot-form').hide();
		
		jQuery('#forgot-password').click(function() {
            jQuery('.login-form').hide();
            jQuery('.forgot-form').show();
        });

        jQuery('#back-btn').click(function() {
            jQuery('.login-form').show();
            jQuery('.forgot-form').hide();
        });

    }
    
    return {
        //main function to initiate the module
        init: function() {

            handleLogin();
            handleForgotPassword();
        }
    };
}();

jQuery(document).ready(function() {
	Auth.init();
});