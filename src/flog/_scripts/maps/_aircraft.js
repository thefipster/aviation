
  export var aircraftPosition = () => {
    return new Promise((resolve, reject)=>{
        const client = new XMLHttpRequest();
        client.open("GET", "/assets/api/park-position.json");
        client.onload = function () {
          const response = client.responseText;
          const data = JSON.parse(response);
          resolve(data);
        };
        client.send();
    });
  }