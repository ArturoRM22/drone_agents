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
    # vision: List[Tuple[int, int]]

@app.post("/step")
async def step(input_data: StepInput):
    #logger.info(f"Step: {input_data.steps}")
    #logger.info(f"Step: {input_data.begin}")
    # Access the data passed in the POST request
    parameters = {
        #'vision': input_data.vision,
        'begin': input_data.begin 
    }
    result = droneModel(parameters)  # Pass the data to your function
    
    return result

# Define the input model using Pydantic
class RecognitionInput(BaseModel):
    detected: List[Tuple[str, float]]

@app.post("/recognition")
async def step(input_data: RecognitionInput):
    # Access the data passed in the POST request
    parameters = {
        'detected': input_data.detected,
    }
    # print(parameters.detected[0][0])
    # result = droneModel(parameters)  # Pass the data to your function
    
    return parameters.detected

if __name__ == '__main__':
    import uvicorn
    uvicorn.run(app, host='0.0.0.0', porit=5000)
