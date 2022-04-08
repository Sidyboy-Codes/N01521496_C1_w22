var formHandle = document.forms.newTeacherForm;
formHandle.onsubmit = processForm;

function processForm() {
  var f_name = formHandle.f_name;
  var l_name = formHandle.l_name;
  var empNum = formHandle.empNum;
  var salary = formHandle.salary;

  if (!f_name.value || f_name.value.trim() === "") {
    console.log("error in fname");
    document.getElementById("f_name").style.background = "red";
    document.getElementById("f_name").addEventListener("click", function () {
      colorwhite("f_name");
    });
    return false;
  }
  if (!l_name.value || l_name.value.trim() === "") {
    document.getElementById("l_name").style.background = "red";
    document.getElementById("l_name").addEventListener("click", function () {
      colorwhite("l_name");
    });
    return false;
  }
  if (!empNum.value || empNum.value.trim() === "") {
    document.getElementById("empNum").style.background = "red";
    document.getElementById("empNum").addEventListener("click", function () {
      colorwhite("empNum");
    });
    return false;
  }

  if (!salary.value || salary.value === null) {
    document.getElementById("salary").style.background = "red";
    document.getElementById("salary").addEventListener("click", function () {
      colorwhite("salary");
    });
    return false;
  }

  function colorwhite(id) {
    document.getElementById(id).style.backgroundColor = "white";
  }

}
