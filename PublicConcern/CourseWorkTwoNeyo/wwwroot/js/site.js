// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.      

//using Ajax For Data Get and Set
/*//code Ref https://www.aspsnippets.com/Articles/Set-Session-variable-in-JavaScript-using-jQuery-in-ASPNet-MVC.aspx
*/
//Ajas for deleting or disabling Users By Admin
function ddUser(id, action) {
    var userId = id;
    var actions = action;

    $.ajax({
        url: '/Admin/ddAccount',
        datatype: "JSON",
        type: "POST",
        data: { actions:actions, userId: userId },
        success: function (data) {
            if (data == 1) {
                alert("You must login as an Admin to delete to a cause!")
                window.location = "/Admin/login"
            }
            else if (data == "2") {
                alert("You have successfully disabled the user.");
                window.location.reload();
            }
            else if (data == "3") {
                alert("You have successfully deleted the user.");
                window.location.reload();
            }
        },
        failure: function (xhr) {
            alert("error");
            return null;
        },
        error: function (exception) {
            alert("error");
            swal("Ooops", 'Exeption:' + exception, "error");
        }
    })
}

//Ajas for deleting Cause By Admin
function delCause(id) {
    var CauseId = id;
    
    $.ajax({
        url: '/Admin/deleteCause',
        datatype: "JSON",
        type: "POST",
        data: { CauseId: CauseId },
        success: function (data) {
            if (data == 1) {
                alert("You must login as an Admin to delete to a cause!")
                window.location = "/Admin/login"
            }
            else if (data == "success") {
                alert("You have successfully deleted the Cause.");
                window.location.reload();
            }
        },
        failure: function (xhr) {
            alert("error");
            return null;
        },
        error: function (exception) {
            alert("error");
            swal("Ooops", 'Exeption:' + exception, "error");
        }
    })
}



//Ref for this function
//https://www.w3schools.com/howto/howto_js_copy_clipboard.asp
function copyLink() {
    /* Get the text field */
    var copyText = document.getElementById("causeLink");
    /* Select the text field */
    copyText.select();
    navigator.clipboard.writeText(copyText.value);
    alert("The share link has been copied!");

}
//Wait function
function wait(milliseconds) {
    setTimeout(function () {
    }, milliseconds);
}

//Ajas for signing Cause
$(function () {
    $("#btnSet").click(function () {
        var CauseId = $("#CauseIdText").val();
        var signMsg = $("#SignMsg").val();
        $.ajax({
            url: '/Causes/signToCause',
            datatype: "JSON",
            type: "POST",
            data: { signMsg: signMsg, CauseId: CauseId },
            success: function (data) {
                if (data == 1) {
                    alert("You must login to sign to a cause!")
                    window.location = "/Home/login"
                }
                else if (data == "success") {
                    alert("You have successfully signed to this cause. \n\nShare Cause with your firends and families!");
                    window.location.reload();
                }
            },
            failure: function (xhr) {
                 alert("error");
                return null;
            },
            error: function (exception) {
                swal("Ooops", 'Exeption:' + exception, "error");
            }
        })

    });
});


//Ajas for Commenting on Cause
$(function () {
    $("#CommentBtn").click(function () {
       // alert("I was here")
        var CauseId = $("#CauseIdText").val();
        var comMsg = $("#ComMsg").val();
        if (comMsg == "") {
        }
        else {
            $.ajax({
                url: '/Causes/commentToCause',
                datatype: "JSON",
                type: "POST",
                data: { comMsg: comMsg, CauseId: CauseId },
                success: function (data) {
                    if (data == 1) {
                        alert("You must login to join the conversation!")
                        window.location = "/Home/login"
                    }
                    else if (data == "success") {
                        window.location.reload();
                    }
                },
                failure: function (xhr) {
                    alert("error");
                    return null;
                },
                error: function (exception) {
                    swal("Ooops", 'Exeption:' + exception, "error");
                }
            })
        }
    });
        
});

//Time Ago implementation
$(document).ready(function () {
    jQuery(document).ready(function () {
        jQuery("time.timeago").timeago();
    });
});

//To take up the tinymce text area ccontent into an hidden textarea
//handles the text-area of the page where causes are written and published And Events Within the textarea
//Ref: https://www.tiny.cloud/docs/general-configuration-guide/basic-setu
tinymce.init({
        selector:'#editor',
        menubar: false,
        statusbar: false,
        plugins: 'autoresize print visualblocks anchor autolink charmap code codesample directionality fullpage help hr image imagetools insertdatetime link lists media nonbreaking pagebreak preview print searchreplace table template textpattern toc visualblocks visualchars',
        toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
        /* style */
	style_formats: [
		{title: "Headers", items: [
			{title: "Header 1", format: "h1"},
			{title: "Header 2", format: "h2"},
			{title: "Header 3", format: "h3"},
			{title: "Header 4", format: "h4"},
			{title: "Header 5", format: "h5"},
			{title: "Header 6", format: "h6"}
		]},
		{title: "Inline", items: [
			{title: "Bold", icon: "bold", format: "bold"},
			{title: "Italic", icon: "italic", format: "italic"},
			{title: "Underline", icon: "underline", format: "underline"},
			{title: "Strikethrough", icon: "strikethrough", format: "strikethrough"},
			{title: "Superscript", icon: "superscript", format: "superscript"},
			{title: "Subscript", icon: "subscript", format: "subscript"},
			{title: "Code", icon: "code", format: "code"}
		]},
		{title: "Blocks", items: [
			{title: "Paragraph", format: "p"},
			{title: "Blockquote", format: "blockquote"},
			{title: "Div", format: "div"},
			{title: "Pre", format: "pre"}
		]},
		{title: "Alignment", items: [
			{title: "Left", icon: "alignleft", format: "alignleft"},
			{title: "Center", icon: "aligncenter", format: "aligncenter"},
			{title: "Right", icon: "alignright", format: "alignright"},
			{title: "Justify", icon: "alignjustify", format: "alignjustify"}
		]}
	],
        skin: 'bootstrap',
        toolbar_drawer: 'floating',
        min_height: 200,           
        autoresize_bottom_margin: 16,
        setup: (editor) => {
            editor.save();
            editor.on('init', () => {
                editor.getContainer().style.transition="border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out"
            });
            editor.on('focus', () => {
                editor.getContainer().style.boxShadow="0 0 0 .2rem rgba(0, 123, 255, .25)",
                editor.getContainer().style.borderColor="#80bdff"
            });
            editor.on('blur', () => {
                editor.getContainer().style.boxShadow="",
                editor.getContainer().style.borderColor=""
            });
            editor.on('input', function (e) {
                var storycontent = editor.getContent();
                var storycontent2 = editor.getContent({ format: 'text' });
                $("#displayContent").val(storycontent);
                $("#displayContent2").val(storycontent2);
            });
        }
    });
    
// Start upload preview image for Cause Registration
//Ref: https://stackoverflow.com/questions/53986538/cropped-image-encrypted-data-not-post-in-codeigniter-on-live-server-why
    // $(".gambar").attr("src", "images/img-demo.jpg");
    var uploadCrop,
    tempFilename,
    rawImg,
    imageId;
var imgW = 300;
var imgH = 150;

if ($("#img-width").val() == "100") {
    imgW = 200;
    imgH = 200;
    //alert("True!" + $("#img-width").val())
}
else {
    //alert("False!" + $("#img-width").val())
}
    function readFile(input) {
        //alert('Shown pop');
        
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('.upload-demo').addClass('ready');
                $('#cropImagePop').modal('show');
                rawImg = e.target.result;
            };
            reader.readAsDataURL(input.files[0]);
        }
        else {
            swal("Sorry - you're browser doesn't support the FileReader API");
        }
    }
    
    uploadCrop = $('#upload-demo').croppie({
        viewport: {
            width: imgW,
            height: imgH,
        },
        enforceBoundary: false,
        enableExif: true,
    });
    
    
    $('#cropImagePop').on('shown.bs.modal', function(){
        // alert('Shown pop');
        uploadCrop.croppie('bind', {
            url: rawImg
        }).then(function(){
            console.log('jQuery bind complete');
        });
    });
    
    $('#item-img').on('change', function () { 
        imageId = $(this).data('id'); tempFilename = $(this).val();
        $('#cancelCropBtn').data('id', imageId); 
        readFile(this);
    });
    $('#cropImageBtn').on('click', function (ev) {
        uploadCrop.croppie('result', {
            type: 'base64', 
            format: 'jpeg',
            size: {width: imgW, height: imgH}
        }).then(function (resp) {
            $('#item-img-output').attr('src', resp);
            $('#round-profile-img').attr('src', resp);
            $('#CauseBannerBox').val(resp)
            $('#cropImagePop').modal('hide');
        });
    });

    // End upload preview image


//Codes for checking password checks and password match
//Ref: https://www.section.io/engineering-education/password-strength-checker-javascript/
// timeout before a callback is called

let timeout;

// traversing the DOM and getting the input and span using their IDs

let password = document.getElementById('confirmPass1')
let strengthBadge = document.getElementById('StrengthDisp')
let strengthBadge2 = document.getElementById('StrengthDisp2')

// The strong and weak password Regex pattern checker

let strongPassword = new RegExp('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})')
let mediumPassword = new RegExp('((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{6,}))|((?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?=.{8,}))')

function StrengthChecker(PasswordParameter) {
    // We then change the badge's color and text based on the password strength

    if (strongPassword.test(PasswordParameter)) {
        strengthBadge.style.backgroundColor = "green"
        strengthBadge.textContent = 'Strong'
    } else if (mediumPassword.test(PasswordParameter)) {
        strengthBadge.style.backgroundColor = 'blue'
        strengthBadge.textContent = 'Medium'
    } else {
        strengthBadge.style.backgroundColor = 'red'
        strengthBadge.textContent = 'Weak'
    }
}

// Adding an input event listener when a user types to the  password input 

password.addEventListener("input", () => {

    //The badge is hidden by default, so we show it

    strengthBadge.style.display = 'block'
    clearTimeout(timeout);

    //We then call the StrengChecker function as a callback then pass the typed password to it

    timeout = setTimeout(() => StrengthChecker(password.value), 500);

    //Incase a user clears the text, the badge is hidden again

    if (password.value.length !== 0) {
        strengthBadge.style.display != 'block'
    } else {
        strengthBadge.style.display = 'none'
    }
});

    //Validate to see if Password is same As Confirm Password For User Registration

$("#confirmPass2").on('input', function () {

    document.getElementById("signupBtn").disabled = true;
    //Assigning Form Objects to Variable
    var password1 = document.getElementById("confirmPass2").value
    var x = $("#confirmPass1").val()
    strengthBadge2.style.display = 'block'

    //Verify if they are the same c
    if (password1.length == 0) {
        strengthBadge2.style.display = 'none'
    }
    else if(x == password1) {
        strengthBadge2.style.backgroundColor = "green"
        strengthBadge2.textContent = 'Password Matched'
        document.getElementById("signupBtn").disabled = false;
    }
    else {
        strengthBadge2.style.backgroundColor = 'red'
        strengthBadge2.textContent = 'Password Does Not Match'
    }
})


