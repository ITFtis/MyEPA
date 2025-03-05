//限定副檔名(accept) 挑選檔案有在accept
function InputFileAccept(objInput) {

    var result = false;

    var exts = $(objInput).attr('accept').split(',');
    var fName = objInput.files[0].name;
    var fExt = fName.substr(fName.lastIndexOf('.'), fName.length);

    $(exts).each(function () {
        var ext = this.trim();
        if (fExt.toLowerCase() == ext.toLowerCase()) {
            result = true;
            return false;
        }
    });

    return result;
}