const baseUrl = "http://localhost:20915/Cooler";

//  Initialize: Data
Vue.createApp({
    // Vue 的 元素包含 1）data
    data(){
        return{
            coolers: null,
            cooler: null,
            coolerPost: {location:null, capacity:0, storage:0, temp:0},
            coolerUpdate: {location:null, capacity:0, storage:0, temp:0},
            sort: 0,
            //isActive:true,
            isFull: false,
            isEmpty: false,
            id: 0,
            addMessage: ""
            //color: text-primary            
            /*
            conditionalFormatSettings: [
                {
                    measure: 'stock',
                    value1: 4,
                    conditions: 'LessThan',
                    style: {
                        backgroundColor: '#80cbc4',
                        color: 'black',
                        fontFamily: 'Tahoma',
                        fontSize: '12px'
                    }
                },
            ]
            */
        }
    },

    // 2）装填 数据
    mounted(){  
        this.GetAll()      
    },

// 3）Methods
    methods: {        
        // GetAll : no filter
        async GetAll(){
            const response = await axios.get(baseUrl)
            this.coolers = response.data                    
        }, 

        async GetById(){
            const response = await axios.get(baseUrl + "/" + this.id)            
            this.cooler = response.data                   
        },

        async AddWine(id){
            const response = await axios.put(baseUrl + "/addWine?id=" + id)    
            try{
                this.addMessage = "response " + response.status + " " + response.statusText
            } catch (ex){
                alert(ex.message)
            }           
            this.coolers = response.data                   
        },

        // after adding new item, please GetAll first before new sorting
        async SortByStorage(){
            if (this.sort == 0){
                this.coolers.sort((a, b) => a.storage - b.storage)
            }
            if(this.sort == 1){
                this.coolers.reverse()
            }
            this.sort = 1                  
        },  
        
        // from query
        async FilterByCapacity(){
            const response = await axios.get(baseUrl + "/filter?capacity=" + this.capacity)            
            this.coolers = response.data   
        },

        async FilterByLocation(){
            const response = await axios.get(baseUrl + "/location?location=" + this.location)            
            this.coolers = response.data   
        },

        /*
        async ShowStatus(){
            GetAll()
            Array.from(coolers).forEach(cooler=> {
                if(cooler.storage == 0){
                    isEmpty = true;                
                } 
                else if(cooler.storage == cooler.capacity){
                    isFull = true;
                }          
                else{
                    isFull = false;                    
                }                
            });
            
            this.coolers = response.data             
        },*/

        async AddNewCooler(){
            await axios.post(baseUrl,this.coolerPost)
            this.GetAll()
            
            // this.id++            
            // cooler = {id: this.id, capacity: this.capacity, storage: this.storage, temp: this.temp}
            // this.coolers.push(cooler)
        },
        
        async UpdateCooler(){
            await axios.put(baseUrl + "/" + this.id, this.coolerUpdate) 
            this.GetAll()                            
        },
        
        // Delete by Query
        async DeleteById(id){
            await axios.delete(baseUrl + "/delete?id=" + id) 
            this.GetAll()                            
        }
        /*,       
        

        async status(cooler){            
            if(cooler.storage == 0) return "Empty"
            else if (cooler.storage == cooler.capacity) return "Full"
            else return "Not Full"                            
        },

        async getColor(cooler){            
            if(cooler.storage == 0) return "text-success"
            else if (cooler.storage == cooler.capacity) return "text-danger"
            else return "text-primary"                            
        } 
        */       
    }
}).mount("#app")  // 4）mount


