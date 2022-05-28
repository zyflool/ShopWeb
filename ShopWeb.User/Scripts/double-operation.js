function add_A_B(A, B) {
    var r1, r2, m, c;
    try {
        r1 = A.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = B.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    c = Math.abs(r1 - r2);
    m = Math.pow(10, Math.max(r1, r2));
    if (c > 0) {
        var cm = Math.pow(10, c);
        if (r1 > r2) {
            A = Number(A.toString().replace(".", ""));
            B = Number(B.toString().replace(".", "")) * cm;
        } else {
            A = Number(A.toString().replace(".", "")) * cm;
            B = Number(B.toString().replace(".", ""));
        }
    } else {
        A = Number(A.toString().replace(".", ""));
        B = Number(B.toString().replace(".", ""));
    }
    return (A + B) / m;
}

function substract_A_B(A, B) {
    var r1, r2, m, n;
    try {
        r1 = A.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = B.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2)); //last modify by deeka //动态控制精度长度
    n = (r1 >= r2) ? r1 : r2;
    return ((A * m - B * m) / m).toFixed(n);

}

function multiply_A_B(A, B) {
    var m = 0, s1 = A.toString(), s2 = B.toString();
    try {
        m += s1.split(".")[1].length;
    }
    catch (e) {
    }
    try {
        m += s2.split(".")[1].length;
    }
    catch (e) {
    }
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
}

function divide_A_B(A, B) {
    var t1 = 0, t2 = 0, r1, r2;
    try {
        t1 = A.toString().split(".")[1].length;
    }
    catch (e) {
    }
    try {
        t2 = B.toString().split(".")[1].length;
    }
    catch (e) {
    }
    with (Math) {
        r1 = Number(A.toString().replace(".", ""));
        r2 = Number(B.toString().replace(".", ""));
        return (r1 / r2) * pow(10, t2 - t1);
    }
}