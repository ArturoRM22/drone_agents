from fastapi import FastAPI
from pydantic import BaseModel
import logging
from typing import List, Tuple
from droneController import droneModel 

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI()

# Define the input model using Pydantic
class StepInput(BaseModel):
    begin: int
    detected: str
    # vision: List[Tuple[int, int]]

@app.post("/step")
async def step(input_data: StepInput):
    print(f"input data: {input_data}")
    parameters = {
        #'vision': input_data.vision,
        'begin': input_data.begin, 
        'detected': int(input_data.detected)
    }
    result = droneModel(parameters)  # Pass the data to your function
    
    return result