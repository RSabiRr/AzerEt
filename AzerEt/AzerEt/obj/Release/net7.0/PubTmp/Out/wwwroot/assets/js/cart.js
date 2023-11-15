var check = false;

changeTotal();


function changeVal(el) {
  var qt = parseFloat(el.parent().children(".qt").html());
  var price = parseFloat(el.parent().children(".price").html());
  var eq = Math.round(price * qt * 100) / 100;
  
    el.parent().children(".full-price").html(eq + "₼" );
  
  changeTotal();      
}

function myFunction() {
    alert("Minumum 15 AZN Sifariş qəbul olunur!");
}

function changeTotal() {
  
  var price = 0;
  
  $(".full-price").each(function(index){
    price += parseFloat($(".full-price").eq(index).html());
  });
    price = Math.round(price * 100) / 100;
    var tax = 3;
    if (price>=50) {
        var tax = 0;
    }  

  var shipping = parseFloat($(".shipping span").html());
  var fullPrice = Math.round((price + tax ) *100) / 100;
  
  if(price == 0) {
    fullPrice = 0;
    }

  

    if (price<15) {
        //document.getElementById("myBtn").disabled = true;
        document.getElementById("myBtn").style.display = 'block';
        document.getElementById("sil").style.display = 'none';
    } else {
        document.getElementById("myBtn").disabled = false;
        document.getElementById("myBtn").style.display = 'none';
        document.getElementById("sil").style.display = 'block';


    }

    $(".subtotal span").html(price);
  $(".tax span").html(tax);

    localStorage.setItem('total', fullPrice);
    
    const ad = localStorage.getItem('total');

    $(".total span").html(fullPrice);


}

