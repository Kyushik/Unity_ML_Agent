# 라이브러리 불러오기
import numpy as np
import random
import time
from collections import deque
from mlagents.envs import UnityEnvironment

# Q-Learning을 위한 파라미터 값 세팅 
state_size = 6
action_size = 4 

load_model = False
train_mode = True

discount_factor = 0.9
learning_rate = 0.1

run_step = 100000
test_step = 5000

print_episode = 100

epsilon_init = 1.0
epsilon_min = 0.1

# 소코반 환경 설정 (게임판 크기=4)
env_config = {"gridSize": 4}

# 유니티 환경 경로 
game = "GridWorld"
env_name = "../env/" + game + "/Windows/" + game

# Q_Agent 클래스 -> Q-Learining 알고리즘을 위한 다양한 함수 정의 
class Q_Agent():
    def __init__(self):
        # 클래스의 함수들을 위한 값 설정 
        self.Q_table = {}
   
        self.epsilon = epsilon_init

    # Q-table에 state 정보가 없는 경우 state 정보 초기화
    def init_Q_table(self, state):
        if state not in self.Q_table.keys():
            self.Q_table[state] = np.zeros(action_size)

    # Epsilon greedy 기법에 따라 행동 결정
    def get_action(self, state):
        if self.epsilon > np.random.rand():
            # 랜덤하게 행동 결정
            return np.random.randint(0, action_size)
        else:
            self.init_Q_table(state)

            # Q-table 연산에 따라 행동 결정
            predict = np.argmax(self.Q_table[state])
            return predict

    # 학습 수행 
    def train_model(self, state, action, reward, next_state, done):
        self.init_Q_table(state)
        self.init_Q_table(next_state)

        # target값 계산 및 Q-Table 업데이트
        target = reward + discount_factor * np.max(self.Q_table[next_state])
        Q_val = self.Q_table[state][action]

        if done:
            self.Q_table[state][action] = reward
        else:
            self.Q_table[state][action] = (1-learning_rate) * Q_val + learning_rate * target
        
# Main 함수 -> 전체적으로 DQN 알고리즘을 진행 
if __name__ == '__main__':
    # 유니티 환경 경로 설정 (file_name)
    env = UnityEnvironment(file_name=env_name)

    # 유니티 브레인 설정 
    default_brain = env.brain_names[0]
    brain = env.brains[default_brain]

    # Q_Agent 클래스를 agent로 정의 
    agent = Q_Agent()

    step = 0
    episode = 0
    reward_list = []

    # 환경 설정 (env_config)에 따라 유니티 환경 리셋 및 학습 모드 설정  
    env_info = env.reset(train_mode=train_mode, config=env_config)[default_brain]

    # 게임 진행 반복문 
    while step < run_step + test_step:         
        # 상태, episode_rewards, done 초기화 
        state = str(env_info.vector_observations[0])
        episode_rewards = 0
        done = False

        # 한 에피소드를 진행하는 반복문 
        while not done:
            if step == run_step:
                train_mode = False
                env_info = env.reset(train_mode=train_mode)[default_brain]

            # 행동 결정 및 유니티 환경에 행동 적용 
            action = agent.get_action(state)
            env_info = env.step(action)[default_brain]

            # 다음 상태, 보상, 게임 종료 정보 취득 
            next_state = str(env_info.vector_observations[0])
            reward = env_info.rewards[0]
            episode_rewards += reward
            done = env_info.local_done[0]

            # 학습 모드인 경우 Q-table 업데이트 
            if train_mode:
                # Epsilon 감소 
                if agent.epsilon > epsilon_min:
                    agent.epsilon -= 1 / run_step

                agent.train_model(state, action, reward, next_state, done)
            else:
                time.sleep(0.01) 
                agent.epsilon = 0.0

            # 상태 정보 업데이트 
            state = next_state
            step += 1

        reward_list.append(episode_rewards)
        episode += 1

        # 진행상황 출력 
        if episode != 0 and episode % print_episode == 0:
            print("Step: {} / Episode: {} / Epsilon: {:.3f} / Mean Rewards: {:.3f}".format(step, episode, agent.epsilon, np.mean(reward_list)))
            reward_list = []

    env.close()




